using Domain.Models.SetGenerationMst;
using Domain.Models.SetMst;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using Helper.Redis;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Infrastructure.Repositories
{
    public class SetGenerationMstRepository : RepositoryBase, ISetGenerationMstRepository
    {
        private readonly StackExchange.Redis.IDatabase _cache;
        private readonly string key;
        private readonly string keySetKbn;
        private readonly IConfiguration _configuration;
        public SetGenerationMstRepository(ITenantProvider tenantProvider, IConfiguration configuration) : base(tenantProvider)
        {
            key = GetCacheKey() + "SetGenerationMst";
            _configuration = configuration;
            GetRedis();
            _cache = RedisConnectorHelper.Connection.GetDatabase();
            keySetKbn = tenantProvider.GetDomainName() + "-" + nameof(SetKbnMstRepository) + "SetKbn";
        }

        public void GetRedis()
        {
            string connection = string.Concat(_configuration["Redis:RedisHost"], ":", _configuration["Redis:RedisPort"]);
            if (RedisConnectorHelper.RedisHost != connection)
            {
                RedisConnectorHelper.RedisHost = connection;
            }
        }

        public IEnumerable<SetGenerationMstModel> ReloadCache(int hpId, bool flag = false)
        {
            var setGenerationMstList = NoTrackingDataContext.SetGenerationMsts.Where(s => s.HpId == hpId && s.IsDeleted == 0).Select(s =>
                    new SetGenerationMstModel(
                        s.HpId,
                        s.GenerationId,
                        s.StartDate,
                        s.IsDeleted
                    )
                  ).ToList();

            var json = JsonSerializer.Serialize(setGenerationMstList);
            _cache.StringSet(key, json);
            if (flag)
            {
                _cache.KeyDelete(keySetKbn);
            }
            return setGenerationMstList;
        }

        public IEnumerable<SetGenerationMstModel> GetList(int hpId, int sinDate)
        {
            IEnumerable<SetGenerationMstModel> setGenerationMstList;
            if (!_cache.KeyExists(key))
            {
                setGenerationMstList = ReloadCache(hpId);
            }
            else
            {
                setGenerationMstList = ReadCache();
            }

            return setGenerationMstList!.Where(s => s.StartDate <= sinDate).OrderByDescending(x => x.StartDate).ToList();
        }

        public List<SetGenerationMstModel> GetSetGenerationMstList(int hpId)
        {
            var setGenerationMstList = ReloadCache(hpId).OrderByDescending(x => x.StartDate).ToList();
            return setGenerationMstList;
        }

        private List<SetGenerationMstModel> ReadCache()
        {
            var results = _cache.StringGet(key);
            var json = results.AsString();
            var datas = JsonSerializer.Deserialize<List<SetGenerationMstModel>>(json);
            return datas ?? new();
        }

        public int GetGenerationId(int hpId, int sinDate)
        {
            int generationId = 0;
            var setGenerationMstList = GetList(hpId, sinDate);
            var generation = setGenerationMstList.OrderByDescending(x => x.StartDate).FirstOrDefault();
            if (generation != null)
            {
                generationId = generation.GenerationId;
            }
            return generationId;
        }

        public List<SetSendaiGenerationModel> GetListSendaiGeneration(int hpId)
        {
            var result = new List<SetSendaiGenerationModel>();

            // Get List Data DB
            var setGenerationMsts = NoTrackingDataContext.SetGenerationMsts.Where(x => x.HpId == hpId && x.IsDeleted == 0).OrderByDescending(x => x.StartDate).ToList();
            for (int i = 0; i < setGenerationMsts.Count; i++)
            {
                if (i == 0)
                {
                    result.Add(new SetSendaiGenerationModel(hpId, setGenerationMsts[i].GenerationId, setGenerationMsts[i].StartDate, convertDateDisplay(setGenerationMsts[i].StartDate), 0, convertDateDisplay(0), i, setGenerationMsts[i].CreateDate, convertCreateDateDisplay(setGenerationMsts[i].CreateDate)));
                }
                else
                {
                    DateTime endTimeDate = CIUtil.IntToDate(setGenerationMsts[i - 1].StartDate);
                    endTimeDate = endTimeDate == DateTime.MinValue ? DateTime.MinValue : endTimeDate.AddDays(-1);
                    var endDateInt = CIUtil.DateTimeToInt(endTimeDate);
                    result.Add(new SetSendaiGenerationModel(hpId, setGenerationMsts[i].GenerationId, setGenerationMsts[i].StartDate, convertDateDisplay(setGenerationMsts[i].StartDate), endDateInt, convertDateDisplay(endDateInt), i, setGenerationMsts[i].CreateDate, convertCreateDateDisplay(setGenerationMsts[i].CreateDate)));
                }
            }
            return result;
        }

        private string convertDateDisplay(int date)
        {
            if (date == 0)
            {
                return "xxx/xx";
            }
            if (date > 0)
            {
                var formatDate = CIUtil.IntToDate(date);
                return formatDate.Year.ToString() + "/" + (formatDate.Month > 9 ? formatDate.Month.ToString() : "0" + formatDate.Month.ToString());
            }
            return "";
        }

        private string convertCreateDateDisplay(DateTime date)
        {
            return date.Year.ToString() + "/" + (date.Month > 9 ? date.Month.ToString() : "0" + date.Month.ToString()) + "/" + (date.Day > 9 ? date.Day.ToString() : "0" + date.Day.ToString());
        }

        /// <summary>
        /// Delete SetSenDai
        /// </summary>
        /// <param name="hpId"></param>
        /// <param name="generationId"></param>
        /// <param name="userId"></param>
        /// <param name="startDate"></param>
        /// <returns></returns>
        public bool DeleteSetSenDaiGeneration(int hpId, int generationId, int userId, int startDate)
        {
            DeleteSetGeneration(hpId, generationId, userId);

            DeleteByomeiSetGeneration(hpId, userId, startDate);

            DeleteListSetGeneration(hpId, generationId, userId);
            var saveChanges = TrackingDataContext.SaveChanges() > 0;

            if (saveChanges)
            {
                ReloadCache(hpId);
                _cache.KeyDelete(keySetKbn);
            }

            return saveChanges;
        }

        /// <summary>
        /// Delete SetGenerationMsts
        /// </summary>
        /// <param name="hpId"></param>
        /// <param name="generationId"></param>
        /// <param name="userId"></param>
        private void DeleteSetGeneration(int hpId, int generationId, int userId)
        {
            var ListDataUpdate = new List<SetGenerationMst>();
            var setGenrationCurrent = TrackingDataContext.SetGenerationMsts.FirstOrDefault(x => x.HpId == hpId && x.GenerationId == generationId && x.IsDeleted == DeleteTypes.None);
            if (setGenrationCurrent != null)
            {
                // Update item delete
                setGenrationCurrent.IsDeleted = 1;
                setGenrationCurrent.UpdateDate = CIUtil.GetJapanDateTimeNow();
                setGenrationCurrent.UpdateId = userId;
                setGenrationCurrent.CreateDate = TimeZoneInfo.ConvertTimeToUtc(setGenrationCurrent.CreateDate);
                ListDataUpdate.Add(setGenrationCurrent);
                // Get Item Above and Update
                var itemAbove = TrackingDataContext.SetGenerationMsts.Where(x => x.HpId == hpId && x.StartDate >= setGenrationCurrent.StartDate && x.GenerationId > setGenrationCurrent.GenerationId && x.IsDeleted == DeleteTypes.None).OrderBy(x => x.StartDate).ThenBy(x => x.GenerationId).FirstOrDefault();
                if (itemAbove != null)
                {
                    itemAbove.StartDate = setGenrationCurrent.StartDate;
                    itemAbove.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    itemAbove.UpdateId = userId;
                    itemAbove.CreateDate = TimeZoneInfo.ConvertTimeToUtc(setGenrationCurrent.CreateDate);
                    ListDataUpdate.Add(itemAbove);
                }

                if (ListDataUpdate.Count > 0)
                {
                    TrackingDataContext.SetGenerationMsts.UpdateRange(ListDataUpdate);
                }
            }
        }

        /// <summary>
        /// Detele ByomeiSetGenerationMsts
        /// </summary>
        /// <param name="hpId"></param>
        /// <param name="userId"></param>
        /// <param name="startDate"></param>
        private void DeleteByomeiSetGeneration(int hpId, int userId, int startDate)
        {
            var deleteByomeiSet = NoTrackingDataContext.ByomeiSetGenerationMsts.FirstOrDefault(x =>
                x.HpId == hpId &&
                x.StartDate == startDate &&
                x.IsDeleted == 0);

            if (deleteByomeiSet == null) return;

            var byomeiSetGeneration = TrackingDataContext.ByomeiSetGenerationMsts.FirstOrDefault(x =>
                    x.HpId == hpId &&
                    x.IsDeleted == 0 &&
                    x.GenerationId == deleteByomeiSet.GenerationId);
            var aboveByomeiSetGeneration = TrackingDataContext.ByomeiSetGenerationMsts.Where(x =>
                    x.HpId == hpId &&
                    x.IsDeleted == 0 &&
                    x.StartDate > deleteByomeiSet.StartDate)
                .OrderBy(x => x.StartDate)
                .FirstOrDefault();
            if (byomeiSetGeneration != null)
            {
                if (aboveByomeiSetGeneration != null)
                {
                    aboveByomeiSetGeneration.StartDate = byomeiSetGeneration.StartDate;
                    aboveByomeiSetGeneration.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    aboveByomeiSetGeneration.UpdateId = userId;
                }
                byomeiSetGeneration.IsDeleted = 1;
                byomeiSetGeneration.UpdateDate = CIUtil.GetJapanDateTimeNow();
                byomeiSetGeneration.UpdateId = userId;
            }
        }

        /// <summary>
        /// Delete ListSetGenerationMsts
        /// </summary>
        /// <param name="hpId"></param>
        /// <param name="userId"></param>
        /// <param name="startDate"></param>
        private void DeleteListSetGeneration(int hpId, int userId, int startDate)
        {
            var deleteListSet = NoTrackingDataContext.ListSetGenerationMsts.FirstOrDefault(x =>
                    x.HpId == hpId &&
                    x.StartDate == startDate &&
                    x.IsDeleted == 0);

            if (deleteListSet == null) return;

            var listSetGeneration = TrackingDataContext.ListSetGenerationMsts.FirstOrDefault(x =>
                    x.HpId == hpId &&
                    x.IsDeleted == 0 &&
                    x.GenerationId == deleteListSet.GenerationId);
            var aboveListSetGeneration = TrackingDataContext.ListSetGenerationMsts.Where(x =>
                    x.HpId == hpId &&
                    x.IsDeleted == 0 &&
                    x.StartDate > deleteListSet.StartDate)
                .OrderBy(x => x.StartDate)
                .FirstOrDefault();
            if (listSetGeneration != null)
            {
                if (aboveListSetGeneration != null)
                {
                    aboveListSetGeneration.StartDate = listSetGeneration.StartDate;
                    aboveListSetGeneration.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    aboveListSetGeneration.UpdateId = userId;
                }
                listSetGeneration.IsDeleted = 1;
                listSetGeneration.UpdateDate = CIUtil.GetJapanDateTimeNow();
                listSetGeneration.UpdateId = userId;
            }
        }

        public AddSetSendaiModel? AddSetSendaiGeneration(int userId, int hpId, int startDate)
        {
            // get SendaiGeneration newest
            var itemNewest = NoTrackingDataContext.SetGenerationMsts.Where(x => x.IsDeleted == 0 && x.HpId == hpId).OrderByDescending(x => x.StartDate).ThenByDescending(s => s.GenerationId).FirstOrDefault();
            if (itemNewest?.StartDate == startDate)
            {
                var year = startDate / 10000;
                var yearMonth = startDate / 100;
                var month = yearMonth % year;
                if (month == 12)
                {
                    startDate = (year + 1) * 10000 + 1 * 100 + 1;
                }
                else
                {
                    startDate += 100;
                }
            }
            // Save item Add
            var itemAdd = new SetGenerationMst();
            itemAdd.StartDate = startDate;
            itemAdd.IsDeleted = 0;
            itemAdd.HpId = hpId;
            itemAdd.CreateId = userId;
            itemAdd.CreateDate = CIUtil.GetJapanDateTimeNow();
            itemAdd.UpdateDate = CIUtil.GetJapanDateTimeNow();
            itemAdd.UpdateId = userId;
            itemAdd.CreateMachine = "SmartKarte";
            itemAdd.UpdateMachine = "SmartKarte";
            TrackingDataContext.SetGenerationMsts.Add(itemAdd);
            var checkAdd = TrackingDataContext.SaveChanges();
            if (checkAdd == 0)
            {
                return null;
            }
            else
            {
                // Clone Generation
                var itemAddGet = TrackingDataContext.SetGenerationMsts.Where(x => x.IsDeleted == 0 && x.HpId == hpId && x.StartDate == startDate).OrderByDescending(x => x.StartDate).ThenByDescending(s => s.GenerationId).FirstOrDefault();
                if (itemNewest != null && itemAddGet != null)
                {
                    return new AddSetSendaiModel(itemAddGet.GenerationId, itemNewest.GenerationId);
                }
            }
            return null;
        }

        public GetCountProcessModel GetCountStepProcess(int targetGenerationId, int sourceGenerationId, int hpId, int userId)
        {
            var setMstsBackuped = TrackingDataContext.SetMsts.Where(x =>
            x.HpId == hpId &&
            x.GenerationId == sourceGenerationId).ToList();
            var setMstDict = new Dictionary<int, SetMst>();
            var ListSetMstNew = new Dictionary<int, SetMstModel>();
            for (int i = 0, len = setMstsBackuped.Count; i < len; i++)
            {
                if (!setMstDict.ContainsKey(setMstsBackuped[i].SetCd))
                {
                    setMstDict[setMstsBackuped[i].SetCd] = setMstsBackuped[i];
                }
            }

            var listMstDict = new List<int>();
            for (int i = 0, len = setMstsBackuped.Count; i < len; i++)
            {
                if (!listMstDict.Contains(setMstsBackuped[i].SetCd))
                {
                    listMstDict.Add(setMstsBackuped[i].SetCd);
                }
            }

            var setKbnMstSource = TrackingDataContext.SetKbnMsts.Where(x =>
                x.HpId == hpId &&
                x.GenerationId == sourceGenerationId).ToList();

            var setByomeisSource = TrackingDataContext.SetByomei.Where(setByomei =>
                setByomei.HpId == hpId && listMstDict.Contains(setByomei.SetCd))
                .ToList();

            var setKarteInfsSource = TrackingDataContext.SetKarteInf.Where(setKarteInf =>
                setKarteInf.HpId == hpId && listMstDict.Contains(setKarteInf.SetCd))
                .ToList();

            var setKarteImgInfsSource = TrackingDataContext.SetKarteImgInf.Where(setKarteImgInf =>
                setKarteImgInf.HpId == hpId && listMstDict.Contains(setKarteImgInf.SetCd))
                .ToList();

            var setOdrInfsSource = TrackingDataContext.SetOdrInf.Where(setOdrInf =>
                setOdrInf.HpId == hpId && listMstDict.Contains(setOdrInf.SetCd))
                .ToList();

            var setOdrInfDetailsSource = TrackingDataContext.SetOdrInfDetail.Where(setOdrInfDetail =>
                setOdrInfDetail.HpId == hpId && listMstDict.Contains(setOdrInfDetail.SetCd))
                .ToList();

            var setOdrInfCmtSource = TrackingDataContext.SetOdrInfCmt.Where(setOdrInfCmt =>
                setOdrInfCmt.HpId == hpId && listMstDict.Contains(setOdrInfCmt.SetCd))
                .ToList();

            // setMsts
            setMstsBackuped.ForEach(x =>
            {
                x.SetCd = 0;
                x.GenerationId = targetGenerationId;
                x.CreateDate = CIUtil.GetJapanDateTimeNow();
                x.CreateId = userId;
                x.UpdateDate = CIUtil.GetJapanDateTimeNow();
                x.UpdateId = userId;
            });
            if (setMstsBackuped.Any())
            {
                TrackingDataContext.SetMsts.AddRange(setMstsBackuped);
                TrackingDataContext.SaveChanges();
            }

            foreach (var item in setMstDict)
            {
                var itemNew = new SetMstModel(item.Value.HpId, item.Value.SetCd);
                ListSetMstNew.Add(item.Key, itemNew);
            }
            return new GetCountProcessModel(setMstsBackuped.Count, setKbnMstSource.Count, setByomeisSource.Count, setKarteInfsSource.Count, setKarteImgInfsSource.Count, setOdrInfsSource.Count, setOdrInfDetailsSource.Count, setOdrInfCmtSource.Count, ListSetMstNew, listMstDict);
        }

        public bool SaveCloneMstBackup(int targetGenerationId, int sourceGenerationId, int hpId, int userId)
        {
            var setMstsBackuped = TrackingDataContext.SetMsts.Where(x =>
            x.HpId == hpId &&
            x.GenerationId == sourceGenerationId).ToList();

            // setMsts
            setMstsBackuped.ForEach(x =>
            {
                x.SetCd = 0;
                x.GenerationId = targetGenerationId;
                x.CreateDate = CIUtil.GetJapanDateTimeNow();
                x.CreateId = userId;
                x.UpdateDate = CIUtil.GetJapanDateTimeNow();
                x.UpdateId = userId;
            });
            if (setMstsBackuped.Any())
            {
                TrackingDataContext.SetMsts.AddRange(setMstsBackuped);
                TrackingDataContext.SaveChanges();
                return true;
            }
            return false;
        }

        public bool SaveCloneKbnMst(int targetGenerationId, int sourceGenerationId, int hpId, int userId)
        {
            var setKbnMstSource = NoTrackingDataContext.SetKbnMsts.Where(x =>
                    x.HpId == hpId &&
                    x.GenerationId == sourceGenerationId).ToList();
            //setKbnMst
            setKbnMstSource.ForEach(setKbnMst =>
            {
                setKbnMst.GenerationId = targetGenerationId;
                setKbnMst.CreateDate = CIUtil.GetJapanDateTimeNow();
                setKbnMst.CreateId = userId;
                setKbnMst.UpdateDate = CIUtil.GetJapanDateTimeNow();
                setKbnMst.UpdateId = userId;
            });

            if (setKbnMstSource.Any())
            {
                TrackingDataContext.SetKbnMsts.AddRange(setKbnMstSource);
                try
                {
                    TrackingDataContext.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    TrackingDataContext.SetKbnMsts.RemoveRange(setKbnMstSource);
                    throw;
                }
            }
            return false;
        }

        public bool SaveCloneByomei(int hpId, int userId, Dictionary<int, SetMstModel> setMstDict, List<int> listMstDict)
        {
            try
            {
                var setByomeisSource = NoTrackingDataContext.SetByomei.Where(setByomei =>
                setByomei.HpId == hpId && listMstDict.Contains(setByomei.SetCd))
                .ToList();

                //setByomeis
                setByomeisSource.ForEach(x =>
                {
                    x.SetCd = setMstDict[x.SetCd].SetCd;
                    x.CreateDate = CIUtil.GetJapanDateTimeNow();
                    x.CreateId = userId;
                    x.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    x.UpdateId = userId;
                });
                if (setByomeisSource.Any())
                {
                    TrackingDataContext.SetByomei.AddRange(setByomeisSource);
                    TrackingDataContext.SaveChanges();
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool SaveCloneKarteInf(int hpId, int userId, Dictionary<int, SetMstModel> setMstDict, List<int> listMstDict)
        {
            var setKarteInfsSource = NoTrackingDataContext.SetKarteInf.Where(setKarteInf =>
            setKarteInf.HpId == hpId && listMstDict.Contains(setKarteInf.SetCd))
            .ToList();

            //setKarteInf
            setKarteInfsSource.ForEach(x =>
            {
                x.SetCd = setMstDict[x.SetCd].SetCd;
                x.CreateDate = CIUtil.GetJapanDateTimeNow();
                x.CreateId = userId;
                x.UpdateDate = CIUtil.GetJapanDateTimeNow();
                x.UpdateId = userId;
            });
            if (setKarteInfsSource.Any())
            {
                TrackingDataContext.SetKarteInf.AddRange(setKarteInfsSource);
                try
                {
                    TrackingDataContext.SaveChanges();
                    return true;
                }
                catch
                {
                    TrackingDataContext.SetKarteInf.RemoveRange(setKarteInfsSource);
                    TrackingDataContext.SaveChanges();
                    return false;
                }
            }
            return false;
        }

        public bool SaveCloneKarteImgInf(int hpId, Dictionary<int, SetMstModel> setMstDict, List<int> listMstDict)
        {
            var setKarteImgInfsSource = NoTrackingDataContext.SetKarteImgInf.Where(setKarteImgInf =>
             setKarteImgInf.HpId == hpId && listMstDict.Contains(setKarteImgInf.SetCd))
             .ToList();

            //setKarteImgInf
            setKarteImgInfsSource.ForEach(x =>
            {
                x.Id = 0;
                x.SetCd = setMstDict[x.SetCd].SetCd;
            });
            if (setKarteImgInfsSource.Any())
            {
                TrackingDataContext.SetKarteImgInf.AddRange(setKarteImgInfsSource);
                try
                {
                    TrackingDataContext.SaveChanges();
                    return true;
                }
                catch
                {
                    TrackingDataContext.SetKarteImgInf.RemoveRange(setKarteImgInfsSource);
                    TrackingDataContext.SaveChanges();
                    return false;
                }
            }

            return false;
        }

        public bool SaveCloneOdrInf(int hpId, int userId, Dictionary<int, SetMstModel> setMstDict, List<int> listMstDict)
        {
            var setOdrInfsSource = NoTrackingDataContext.SetOdrInf.Where(setOdrInf =>
             setOdrInf.HpId == hpId && listMstDict.Contains(setOdrInf.SetCd))
             .ToList();

            //setOdrInf
            setOdrInfsSource.ForEach((x) =>
            {
                x.SetCd = setMstDict[x.SetCd].SetCd;
                x.Id = 0;
                x.CreateDate = CIUtil.GetJapanDateTimeNow();
                x.CreateId = userId;
                x.UpdateDate = CIUtil.GetJapanDateTimeNow();
                x.UpdateId = userId;
            });
            if (setOdrInfsSource.Any())
            {
                TrackingDataContext.SetOdrInf.AddRange(setOdrInfsSource);
                try
                {
                    TrackingDataContext.SaveChanges();
                    return true;
                }
                catch
                {
                    TrackingDataContext.SetOdrInf.RemoveRange(setOdrInfsSource);
                    TrackingDataContext.SaveChanges();
                    return false;
                }
            }

            return false;
        }

        public bool SaveCloneOdrInfDetail(int hpId, Dictionary<int, SetMstModel> setMstDict, List<int> listMstDict)
        {
            var setOdrInfDetailsSource = NoTrackingDataContext.SetOdrInfDetail.Where(setOdrInfDetail =>
            setOdrInfDetail.HpId == hpId && listMstDict.Contains(setOdrInfDetail.SetCd))
            .ToList();

            //setOdrInfDetail
            setOdrInfDetailsSource.ForEach((x) =>
            {
                x.SetCd = setMstDict[x.SetCd].SetCd;
            });
            if (setOdrInfDetailsSource.Any())
            {
                TrackingDataContext.SetOdrInfDetail.AddRange(setOdrInfDetailsSource);
                try
                {
                    TrackingDataContext.SaveChanges();
                    return true;
                }
                catch
                {
                    TrackingDataContext.SetOdrInfDetail.RemoveRange(setOdrInfDetailsSource);
                    TrackingDataContext.SaveChanges();
                    return false;
                }
            }

            return false;
        }

        public bool SaveCloneOdrInfCmt(int hpId, Dictionary<int, SetMstModel> setMstDict, List<int> listMstDict)
        {
            var setOdrInfCmtSource = NoTrackingDataContext.SetOdrInfCmt.Where(setOdrInfCmt =>
            setOdrInfCmt.HpId == hpId && listMstDict.Contains(setOdrInfCmt.SetCd))
            .ToList();

            //setOdrInfCmt
            setOdrInfCmtSource.ForEach((x) =>
            {
                x.SetCd = setMstDict[x.SetCd].SetCd;
            });
            if (setOdrInfCmtSource.Any())
            {
                TrackingDataContext.SetOdrInfCmt.AddRange(setOdrInfCmtSource);
                try
                {
                    TrackingDataContext.SaveChanges();
                    return true;
                }
                catch
                {
                    TrackingDataContext.SetOdrInfCmt.RemoveRange(setOdrInfCmtSource);
                    TrackingDataContext.SaveChanges();
                    return false;
                }
            }

            return false;
        }

        public AddSetSendaiModel? RestoreSetSendaiGeneration(int restoreGenerationId, int hpId, int userId)
        {
            // get SendaiGeneration newest
            var itemNewest = TrackingDataContext.SetGenerationMsts.Where(x => x.IsDeleted == 0 && x.HpId == hpId).OrderByDescending(x => x.StartDate).ThenByDescending(x => x.GenerationId).FirstOrDefault();
            if (itemNewest != null && itemNewest.GenerationId != restoreGenerationId)
            {
                // delete newest
                var targetSetMsts = TrackingDataContext.SetMsts.Where(x =>
                        x.HpId == hpId &&
                        x.GenerationId == itemNewest.GenerationId).ToList();

                var targetSetMstsDict = new HashSet<int>();
                for (int i = 0, len = targetSetMsts.Count; i < len; i++)
                {
                    if (!targetSetMstsDict.Contains(targetSetMsts[i].SetCd))
                    {
                        targetSetMstsDict.Add(targetSetMsts[i].SetCd);
                    }
                }

                var setKbnMstSource = TrackingDataContext.SetKbnMsts.Where(x =>
                        x.HpId == hpId &&
                        x.GenerationId == itemNewest.GenerationId).ToList();

                var targetSetByomeis = TrackingDataContext.SetByomei.Where(setByomei =>
                       setByomei.HpId == hpId && targetSetMstsDict.Contains(setByomei.SetCd))
                       .ToList();

                var targetSetKarteInfs = TrackingDataContext.SetKarteInf.Where(setKarteInf =>
                        setKarteInf.HpId == hpId && targetSetMstsDict.Contains(setKarteInf.SetCd))
                        .ToList();

                var targetSetKarteImgInfs = TrackingDataContext.SetKarteImgInf.Where(setKarteImgInf =>
                       setKarteImgInf.HpId == hpId && targetSetMstsDict.Contains(setKarteImgInf.SetCd))
                       .ToList();

                var targetSetOdrInfs = TrackingDataContext.SetOdrInf.Where(setOdrInf =>
                        setOdrInf.HpId == hpId && targetSetMstsDict.Contains(setOdrInf.SetCd))
                        .ToList();

                var targetSetOdrInfDetails = TrackingDataContext.SetOdrInfDetail.Where(setOdrInfDetail =>
                    setOdrInfDetail.HpId == hpId && targetSetMstsDict.Contains(setOdrInfDetail.SetCd))
                    .ToList();
                var targetSetOdrInfCmtSource = TrackingDataContext.SetOdrInfCmt.Where(setOdrInfCmt =>
                     setOdrInfCmt.HpId == hpId && targetSetMstsDict.Contains(setOdrInfCmt.SetCd))
                     .ToList();

                setKbnMstSource.ForEach(setKbnMst =>
                {
                    setKbnMst.IsDeleted = 1;
                    setKbnMst.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    setKbnMst.UpdateId = userId;
                    setKbnMst.UpdateMachine = "SmartKarte";
                });

                targetSetMsts.ForEach(setMst =>
                {
                    setMst.IsDeleted = 1;
                    setMst.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    setMst.UpdateId = userId;
                    setMst.UpdateMachine = "SmartKarte";
                });

                targetSetByomeis.ForEach(setByomei =>
                {
                    setByomei.IsDeleted = 1;
                    setByomei.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    setByomei.UpdateId = userId;
                    setByomei.UpdateMachine = "SmartKarte";
                });

                targetSetKarteInfs.ForEach(setKarteInf =>
                {
                    setKarteInf.IsDeleted = 1;
                    setKarteInf.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    setKarteInf.UpdateId = userId;
                    setKarteInf.UpdateMachine = "SmartKarte";
                });

                targetSetOdrInfs.ForEach(setOdrInf =>
                {
                    setOdrInf.IsDeleted = 1;
                    setOdrInf.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    setOdrInf.UpdateId = userId;
                    setOdrInf.UpdateMachine = "SmartKarte";
                });
                TrackingDataContext.SetKbnMsts.RemoveRange(setKbnMstSource);
                TrackingDataContext.SetKarteInf.RemoveRange(targetSetKarteInfs);
                TrackingDataContext.SetOdrInfCmt.RemoveRange(targetSetOdrInfCmtSource);
                TrackingDataContext.SetKarteImgInf.RemoveRange(targetSetKarteImgInfs);
                TrackingDataContext.SetOdrInfDetail.RemoveRange(targetSetOdrInfDetails);
                TrackingDataContext.SetMsts.RemoveRange(targetSetMsts);
                TrackingDataContext.SaveChanges();
                // clone data from newest to restore item
                return new AddSetSendaiModel(itemNewest.GenerationId, restoreGenerationId);
            }

            return null;
        }

        /// <summary>
        /// Delete ByomiSetGeneraion
        /// </summary>
        /// <param name="hpId"></param>
        /// <param name="userId"></param>
        /// <param name="targetGeneration"></param>
        /// <param name="sourceGenerationId"></param>
        public void RestoreByomeiSetGeneration(int hpId, int userId, int targetGeneration, int sourceGenerationId)
        {
            var targetSetMsts = TrackingDataContext.ByomeiSetMsts.Where(x =>
                            x.HpId == hpId &&
                            x.GenerationId == targetGeneration).ToList();

            var executionStrategy = TrackingDataContext.Database.CreateExecutionStrategy();
            executionStrategy.Execute(
            () =>
            {
                using (var transaction = TrackingDataContext.Database.BeginTransaction())
                {
                    try
                    {
                        targetSetMsts.ForEach(byomeiSetMst =>
                        {
                            byomeiSetMst.IsDeleted = 1;
                            byomeiSetMst.UpdateDate = CIUtil.GetJapanDateTimeNow();
                            byomeiSetMst.UpdateId = userId;
                        });
                        TrackingDataContext.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            });

            CloneByomeiSetGeneration(hpId, userId, targetGeneration, sourceGenerationId);
        }

        /// <summary>
        /// Update GenerationId of ByomeiSetMsts
        /// </summary>
        /// <param name="hpId"></param>
        /// <param name="userId"></param>
        /// <param name="targetGeneration"></param>
        /// <param name="sourceGenerationId"></param>
        public void CloneByomeiSetGeneration(int hpId, int userId, int targetGeneration, int sourceGenerationId)
        {
            var setMstsBackuped = TrackingDataContext.ByomeiSetMsts.Where(x =>
                x.HpId == hpId &&
                x.GenerationId == sourceGenerationId).ToList();
            int stepCount = setMstsBackuped.Count;
            if (stepCount == 0)
            {
                return;
            }

            var executionStrategy = TrackingDataContext.Database.CreateExecutionStrategy();
            executionStrategy.Execute(
            () =>
            {
                using (var transaction = TrackingDataContext.Database.BeginTransaction())
                {
                    var targetSetMsts = TrackingDataContext.ByomeiSetMsts.Where(x =>
                        x.HpId == hpId &&
                        x.GenerationId == targetGeneration).ToList();
                    try
                    {
                        setMstsBackuped.ForEach(x =>
                        {
                            x.SeqNo = 0;
                            x.GenerationId = targetGeneration;
                            x.CreateDate = CIUtil.GetJapanDateTimeNow();
                            x.CreateId = userId;
                            x.UpdateDate = CIUtil.GetJapanDateTimeNow();
                            x.UpdateId = userId;
                        });
                        TrackingDataContext.ByomeiSetMsts.AddRange(setMstsBackuped);
                        TrackingDataContext.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }

                }
            });
        }

        /// <summary>
        /// Delete ListSetMsts
        /// </summary>
        /// <param name="hpId"></param>
        /// <param name="userId"></param>
        /// <param name="targetGeneration"></param>
        /// <param name="sourceGenerationId"></param>
        public void RestoreListSetGeneration(int hpId, int userId, int targetGeneration, int sourceGenerationId)
        {
            var targetSetMsts = TrackingDataContext.ListSetMsts.Where(x =>
                            x.HpId == hpId &&
                            x.GenerationId == targetGeneration).ToList();

            var executionStrategy = TrackingDataContext.Database.CreateExecutionStrategy();
            executionStrategy.Execute(
            () =>
            {
                using (var transaction = TrackingDataContext.Database.BeginTransaction())
                {
                    try
                    {
                        targetSetMsts.ForEach(listSetMst =>
                        {
                            listSetMst.HpId = hpId;
                            listSetMst.IsDeleted = 1;
                            listSetMst.UpdateDate = CIUtil.GetJapanDateTimeNow();
                            listSetMst.UpdateId = userId;
                            listSetMst.UpdateMachine = CIUtil.GetComputerName();
                        });
                        TrackingDataContext.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            });

            CloneListSetGeneration(hpId, userId, targetGeneration, sourceGenerationId);
        }

        /// <summary>
        /// Update Generation Id of ListSetMsts
        /// </summary>
        /// <param name="hpId"></param>
        /// <param name="userId"></param>
        /// <param name="targetGeneration"></param>
        /// <param name="sourceGenerationId"></param>
        public void CloneListSetGeneration(int hpId, int userId, int targetGeneration, int sourceGenerationId)
        {
            var setMstsBackuped = TrackingDataContext.ListSetMsts.Where(x =>
                x.HpId == hpId &&
                x.GenerationId == sourceGenerationId).ToList();
            int stepCount = setMstsBackuped.Count;
            if (stepCount == 0)
            {
                return;
            }

            var executionStrategy = TrackingDataContext.Database.CreateExecutionStrategy();
            executionStrategy.Execute(
            () =>
            {
                using (var transaction = TrackingDataContext.Database.BeginTransaction())
                {
                    try
                    {
                        setMstsBackuped.ForEach(x =>
                        {
                            x.HpId = hpId;
                            x.SetId = 0;
                            x.GenerationId = targetGeneration;
                            x.CreateDate = CIUtil.GetJapanDateTimeNow();
                            x.CreateId = userId;
                            x.UpdateDate = CIUtil.GetJapanDateTimeNow();
                            x.UpdateId = userId;
                        });
                        TrackingDataContext.ListSetMsts.AddRange(setMstsBackuped);
                        TrackingDataContext.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                    }
                }
            });
        }

        public List<ListSetGenerationMstModel> GetAll(int hpId)
        {
            var listSetGenerationMstList = NoTrackingDataContext.ListSetGenerationMsts.Where(s => s.HpId == hpId && s.IsDeleted == 0).OrderByDescending(s => s.StartDate).Select(s =>
                    new ListSetGenerationMstModel(
                        s.HpId,
                        s.GenerationId,
                        s.StartDate,
                        s.IsDeleted
                    )
                  ).ToList();
            return listSetGenerationMstList;
        }

        /// <summary>
        /// Insert Data to ByomeiSetGenerationMsts
        /// </summary>
        /// <param name="hpId"></param>
        /// <param name="userId"></param>
        /// <param name="startDate"></param>
        /// <returns></returns>
        public AddSetSendaiModel? AddByomeiSetGenerationMst(int hpId, int userId, int startDate)
        {
            // get SendaiGeneration newest
            var itemNewest = NoTrackingDataContext.ByomeiSetGenerationMsts.Where(x => x.IsDeleted == 0 && x.HpId == hpId).OrderByDescending(x => x.StartDate).ThenByDescending(s => s.GenerationId).FirstOrDefault();
            if (itemNewest?.StartDate == startDate)
            {
                var year = startDate / 10000;
                var yearMonth = startDate / 100;
                var month = yearMonth % year;
                if (month == 12)
                {
                    startDate = (year + 1) * 10000 + 1 * 100 + 1;
                }
                else
                {
                    startDate += 100;
                }
            }

            //insert data ByomeiSetGenerationMst
            var targetByomeiSet = new ByomeiSetGenerationMst();

            targetByomeiSet.HpId = hpId;
            targetByomeiSet.StartDate = startDate;
            targetByomeiSet.CreateDate = CIUtil.GetJapanDateTimeNow();
            targetByomeiSet.CreateId = userId;
            targetByomeiSet.UpdateDate = CIUtil.GetJapanDateTimeNow();
            targetByomeiSet.UpdateId = userId;
            TrackingDataContext.ByomeiSetGenerationMsts.Add(targetByomeiSet);
            var isSaveChange = TrackingDataContext.SaveChanges() > 0;

            if (!isSaveChange)
            {
                return null;
            }
            else
            {
                // Clone Generation
                var itemAddGet = TrackingDataContext.ByomeiSetGenerationMsts.Where(x => x.IsDeleted == 0 && x.HpId == hpId && x.StartDate == startDate).OrderByDescending(x => x.StartDate).ThenByDescending(s => s.GenerationId).FirstOrDefault();
                if (itemNewest != null && itemAddGet != null)
                {
                    return new AddSetSendaiModel(itemAddGet.GenerationId, itemNewest.GenerationId);
                }
            }

            return null;
        }

        /// <summary>
        /// Insert Data To ListSetGenerationMsts
        /// </summary>
        /// <param name="hpId"></param>
        /// <param name="userId"></param>
        /// <param name="startDate"></param>
        /// <returns></returns>
        public AddSetSendaiModel? AddListSetGenerationMst(int hpId, int userId, int startDate)
        {
            // get ListSetGenerationMst newest
            var itemNewest = NoTrackingDataContext.ListSetGenerationMsts.Where(x => x.IsDeleted == 0 && x.HpId == hpId).OrderByDescending(x => x.StartDate).ThenByDescending(s => s.GenerationId).FirstOrDefault();
            if (itemNewest?.StartDate == startDate)
            {
                var year = startDate / 10000;
                var yearMonth = startDate / 100;
                var month = yearMonth % year;
                if (month == 12)
                {
                    startDate = (year + 1) * 10000 + 1 * 100 + 1;
                }
                else
                {
                    startDate += 100;
                }
            }

            //insert data ListSetGenerationMst
            var targetListSet = new ListSetGenerationMst();

            targetListSet.HpId = hpId;
            targetListSet.StartDate = startDate;
            targetListSet.CreateDate = CIUtil.GetJapanDateTimeNow();
            targetListSet.CreateId = userId;
            targetListSet.UpdateDate = CIUtil.GetJapanDateTimeNow();
            targetListSet.UpdateId = userId;
            TrackingDataContext.ListSetGenerationMsts.Add(targetListSet);
            var isSaveChange = TrackingDataContext.SaveChanges() > 0;

            if (!isSaveChange)
            {
                return null;
            }
            else
            {
                // Clone ListSetGenerationMst
                var itemAddGet = TrackingDataContext.ListSetGenerationMsts.Where(x => x.IsDeleted == 0 && x.HpId == hpId && x.StartDate == startDate).OrderByDescending(x => x.StartDate).ThenByDescending(s => s.GenerationId).FirstOrDefault();
                if (itemNewest != null && itemAddGet != null)
                {
                    return new AddSetSendaiModel(itemAddGet.GenerationId, itemNewest.GenerationId);
                }
            }

            return null;
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
