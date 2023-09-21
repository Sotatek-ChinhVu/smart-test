using Amazon.Runtime.Internal.Transform;
using Domain.Models.SetGenerationMst;
using Domain.Models.SetMst;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class SetGenerationMstRepository : RepositoryBase, ISetGenerationMstRepository
    {
        private readonly IMemoryCache _memoryCache;
        private readonly string _computerName = "SmartKarte";
        public SetGenerationMstRepository(ITenantProvider tenantProvider, IMemoryCache memoryCache) : base(tenantProvider)
        {
            _memoryCache = memoryCache;
        }

        private IEnumerable<SetGenerationMstModel> ReloadCache()
        {
            var setGenerationMstList = NoTrackingDataContext.SetGenerationMsts.Where(s => s.HpId == 1 && s.IsDeleted == 0).Select(s =>
                    new SetGenerationMstModel(
                        s.HpId,
                        s.GenerationId,
                        s.StartDate,
                        s.IsDeleted
                    )
                  ).ToList();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetPriority(CacheItemPriority.Normal);
            _memoryCache.Set(GetCacheKey(), setGenerationMstList, cacheEntryOptions);

            return setGenerationMstList;
        }

        public IEnumerable<SetGenerationMstModel> GetList(int hpId, int sinDate)
        {
            if (!_memoryCache.TryGetValue(GetCacheKey(), out IEnumerable<SetGenerationMstModel>? setGenerationMstList))
            {
                setGenerationMstList = ReloadCache();
            }

            return setGenerationMstList!.Where(s => s.StartDate <= sinDate).OrderByDescending(x => x.StartDate).ToList();
        }

        public int GetGenerationId(int hpId, int sinDate)
        {
            int generationId = 0;
            try
            {
                var setGenerationMstList = GetList(hpId, sinDate);
                var generation = setGenerationMstList.OrderByDescending(x => x.StartDate).FirstOrDefault();
                if (generation != null)
                {
                    generationId = generation.GenerationId;
                }
            }
            catch
            {
                return 0;
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

        public bool DeleteSetSenDaiGeneration(int generationId, int userId)
        {
            var ListDataUpdate = new List<SetGenerationMst>();
            var setGenrationCurrent = TrackingDataContext.SetGenerationMsts.FirstOrDefault(x => x.GenerationId == generationId);
            if (setGenrationCurrent != null)
            {
                // Update item delete
                setGenrationCurrent.IsDeleted = 1;
                setGenrationCurrent.UpdateDate = CIUtil.GetJapanDateTimeNow();
                setGenrationCurrent.UpdateId = userId;
                setGenrationCurrent.CreateDate = TimeZoneInfo.ConvertTimeToUtc(setGenrationCurrent.CreateDate);
                ListDataUpdate.Add(setGenrationCurrent);
                // Get Item Above and Update
                var itemAbove = TrackingDataContext.SetGenerationMsts.Where(x => x.StartDate > setGenrationCurrent.StartDate).OrderBy(x => x.StartDate).FirstOrDefault();
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
                    return TrackingDataContext.SaveChanges() > 0;
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        public AddSetSendaiModel? AddSetSendaiGeneration(int userId, int hpId, int startDate)
        {
            // get SendaiGeneration newest
            var itemNewest = TrackingDataContext.SetGenerationMsts.Where(x => x.IsDeleted == 0 && x.HpId == hpId).OrderByDescending(x => x.StartDate).FirstOrDefault();
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
                var itemAddGet = TrackingDataContext.SetGenerationMsts.Where(x => x.IsDeleted == 0 && x.HpId == hpId && x.StartDate == startDate).OrderByDescending(x => x.StartDate).FirstOrDefault();
                if (itemNewest != null && itemAddGet != null)
                {
                    return new AddSetSendaiModel(itemAddGet.GenerationId, itemNewest.GenerationId); ;
                }
            }
            return null;
        }

        public GetCountProcessModel GetCountStepProcess(int targetGenerationId, int sourceGenerationId, int hpId, int userId)
        {
            try
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
                    x.CreateMachine = _computerName;
                    x.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    x.UpdateId = userId;
                    x.UpdateMachine = _computerName;
                });
                if (setMstsBackuped.Any())
                {
                    TrackingDataContext.SetMsts.AddRange(setMstsBackuped);
                    TrackingDataContext.SaveChanges();
                }

                foreach (var item in setMstDict)
                {
                    var key = item.Key;
                    var itemNew = new SetMstModel(item.Value.HpId, item.Value.SetCd);
                    ListSetMstNew.Add(key, itemNew);
                }
                return new GetCountProcessModel(setMstsBackuped.Count, setKbnMstSource.Count, setByomeisSource.Count, setKarteInfsSource.Count, setKarteImgInfsSource.Count, setOdrInfsSource.Count, setOdrInfDetailsSource.Count, setOdrInfCmtSource.Count, ListSetMstNew, listMstDict);
            }
            catch
            {
                return new GetCountProcessModel();
            }
        }

        public bool SaveCloneMstBackup(int targetGenerationId, int sourceGenerationId, int hpId, int userId)
        {
            try
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
                    x.CreateMachine = _computerName;
                    x.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    x.UpdateId = userId;
                    x.UpdateMachine = _computerName;
                });
                if (setMstsBackuped.Any())
                {
                    TrackingDataContext.SetMsts.AddRange(setMstsBackuped);
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


        public bool SaveCloneKbnMst(int targetGenerationId, int sourceGenerationId, int hpId, int userId)
        {
            try
            {
                var setKbnMstSource = TrackingDataContext.SetKbnMsts.Where(x =>
                    x.HpId == hpId &&
                    x.GenerationId == sourceGenerationId).ToList();

                //setKbnMst
                setKbnMstSource.ForEach(setKbnMst =>
                {
                    setKbnMst.GenerationId = targetGenerationId;
                    setKbnMst.CreateDate = CIUtil.GetJapanDateTimeNow();
                    setKbnMst.CreateId = userId;
                    setKbnMst.CreateMachine = _computerName;
                    setKbnMst.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    setKbnMst.UpdateId = userId;
                    setKbnMst.UpdateMachine = _computerName;
                });

                if (setKbnMstSource.Any())
                {
                    TrackingDataContext.SetKbnMsts.AddRange(setKbnMstSource);
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

        public bool SaveCloneByomei(int hpId, int userId, Dictionary<int, SetMstModel> setMstDict, List<int> listMstDict)
        {
            try
            {
                var setByomeisSource = TrackingDataContext.SetByomei.Where(setByomei =>
                setByomei.HpId == hpId && listMstDict.Contains(setByomei.SetCd))
                .ToList();

                //setByomeis
                setByomeisSource.ForEach(x =>
                {
                    x.SetCd = setMstDict[x.SetCd].SetCd;
                    x.CreateDate = CIUtil.GetJapanDateTimeNow();
                    x.CreateId = userId;
                    x.CreateMachine = _computerName;
                    x.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    x.UpdateId = userId;
                    x.UpdateMachine = _computerName;
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
            try
            {
               var setKarteInfsSource = TrackingDataContext.SetKarteInf.Where(setKarteInf =>
               setKarteInf.HpId == hpId && listMstDict.Contains(setKarteInf.SetCd))
               .ToList();

                //setKarteInf
                setKarteInfsSource.ForEach(x =>
                {
                    x.SetCd = setMstDict[x.SetCd].SetCd;
                    x.CreateDate = CIUtil.GetJapanDateTimeNow();
                    x.CreateId = userId;
                    x.CreateMachine = _computerName;
                    x.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    x.UpdateId = userId;
                    x.UpdateMachine = _computerName;
                });
                if (setKarteInfsSource.Any())
                {
                    TrackingDataContext.SetKarteInf.AddRange(setKarteInfsSource);
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

        public bool SaveCloneKarteImgInf(int hpId, Dictionary<int, SetMstModel> setMstDict, List<int> listMstDict)
        {
            try
            {
                var setKarteImgInfsSource = TrackingDataContext.SetKarteImgInf.Where(setKarteImgInf =>
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

        public bool SaveCloneOdrInf(int hpId, int userId, Dictionary<int, SetMstModel> setMstDict, List<int> listMstDict)
        {
            try
            {
                var setOdrInfsSource = TrackingDataContext.SetOdrInf.Where(setOdrInf =>
                 setOdrInf.HpId == hpId && listMstDict.Contains(setOdrInf.SetCd))
                 .ToList();

                //setOdrInf
                setOdrInfsSource.ForEach((x) =>
                {
                    x.SetCd = setMstDict[x.SetCd].SetCd;
                    x.Id = 0;
                    x.CreateDate = CIUtil.GetJapanDateTimeNow();
                    x.CreateId = userId;
                    x.CreateMachine = _computerName;
                    x.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    x.UpdateId = userId;
                    x.UpdateMachine = _computerName;
                });
                if (setOdrInfsSource.Any())
                {
                    TrackingDataContext.SetOdrInf.AddRange(setOdrInfsSource);
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

        public bool SaveCloneOdrInfDetail(int hpId, Dictionary<int, SetMstModel> setMstDict, List<int> listMstDict)
        {
            try
            {
                var setOdrInfDetailsSource = TrackingDataContext.SetOdrInfDetail.Where(setOdrInfDetail =>
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

        public bool SaveCloneOdrInfCmt(int hpId, Dictionary<int, SetMstModel> setMstDict, List<int> listMstDict)
        {
            try
            {
                var setOdrInfCmtSource = TrackingDataContext.SetOdrInfCmt.Where(setOdrInfCmt =>
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

        public void CloneGeneration(int targetGenerationId, int sourceGenerationId, int hpId, int userId)
        {
            var setMstsBackuped = TrackingDataContext.SetMsts.Where(x =>
                x.HpId == hpId &&
                x.GenerationId == sourceGenerationId).ToList();
            var setMstDict = new Dictionary<int, SetMst>();
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

            int countData = setMstsBackuped.Count + setByomeisSource.Count + setKarteInfsSource.Count + setKarteImgInfsSource.Count +
                            setOdrInfsSource.Count + setOdrInfDetailsSource.Count + setOdrInfCmtSource.Count + setKbnMstSource.Count;
            // add process save data
            var computerName = "SmartKarte";
            // setMsts
            setMstsBackuped.ForEach(x =>
            {
                x.SetCd = 0;
                x.GenerationId = targetGenerationId;
                x.CreateDate = CIUtil.GetJapanDateTimeNow();
                x.CreateId = userId;
                x.CreateMachine = computerName;
                x.UpdateDate = CIUtil.GetJapanDateTimeNow();
                x.UpdateId = Session.UserID;
                x.UpdateMachine = computerName;
            });
            if (setMstsBackuped.Any())
            {
                TrackingDataContext.SetMsts.AddRange(setMstsBackuped);
                TrackingDataContext.SaveChanges();
            }


            //setKbnMst
            setKbnMstSource.ForEach(setKbnMst =>
            {
                setKbnMst.GenerationId = targetGenerationId;
                setKbnMst.CreateDate = CIUtil.GetJapanDateTimeNow();
                setKbnMst.CreateId = Session.UserID;
                setKbnMst.CreateMachine = computerName;
                setKbnMst.UpdateDate = CIUtil.GetJapanDateTimeNow();
                setKbnMst.UpdateId = Session.UserID;
                setKbnMst.UpdateMachine = computerName;
            });
            if (setKbnMstSource.Any())
            {
                TrackingDataContext.SetKbnMsts.AddRange(setKbnMstSource);
            }

            //setByomeis
            setByomeisSource.ForEach(x =>
            {
                x.SetCd = setMstDict[x.SetCd].SetCd;
                x.CreateDate = CIUtil.GetJapanDateTimeNow();
                x.CreateId = Session.UserID;
                x.CreateMachine = computerName;
                x.UpdateDate = CIUtil.GetJapanDateTimeNow();
                x.UpdateId = Session.UserID;
                x.UpdateMachine = computerName;
            });
            if (setByomeisSource.Any())
            {
                TrackingDataContext.SetByomei.AddRange(setByomeisSource);
            }

            //setKarteInf
            setKarteInfsSource.ForEach(x =>
            {
                x.SetCd = setMstDict[x.SetCd].SetCd;
                x.CreateDate = CIUtil.GetJapanDateTimeNow();
                x.CreateId = Session.UserID;
                x.CreateMachine = computerName;
                x.UpdateDate = CIUtil.GetJapanDateTimeNow();
                x.UpdateId = Session.UserID;
                x.UpdateMachine = computerName;
            });
            if (setKarteInfsSource.Any())
            {
                TrackingDataContext.SetKarteInf.AddRange(setKarteInfsSource);
            }

            //setKarteImgInf
            setKarteImgInfsSource.ForEach(x =>
            {
                x.Id = 0;
                x.SetCd = setMstDict[x.SetCd].SetCd;
            });
            if (setKarteImgInfsSource.Any())
            {
                TrackingDataContext.SetKarteImgInf.AddRange(setKarteImgInfsSource);
            }

            //setOdrInf
            setOdrInfsSource.ForEach((x) =>
            {
                x.SetCd = setMstDict[x.SetCd].SetCd;
                x.Id = 0;
                x.CreateDate = CIUtil.GetJapanDateTimeNow();
                x.CreateId = Session.UserID;
                x.CreateMachine = computerName;
                x.UpdateDate = CIUtil.GetJapanDateTimeNow();
                x.UpdateId = Session.UserID;
                x.UpdateMachine = computerName;
            });
            if (setOdrInfsSource.Any())
            {
                TrackingDataContext.SetOdrInf.AddRange(setOdrInfsSource);
            }

            //setOdrInfDetail
            setOdrInfDetailsSource.ForEach((x) =>
            {
                x.SetCd = setMstDict[x.SetCd].SetCd;
            });
            if (setOdrInfDetailsSource.Any())
            {
                TrackingDataContext.SetOdrInfDetail.AddRange(setOdrInfDetailsSource);
            }

            //setOdrInfCmt
            setOdrInfCmtSource.ForEach((x) =>
            {
                x.SetCd = setMstDict[x.SetCd].SetCd;
            });
            if (setOdrInfCmtSource.Any())
            {
                TrackingDataContext.SetOdrInfCmt.AddRange(setOdrInfCmtSource);
            }

            if (countData > 0)
            {
                TrackingDataContext.SaveChanges();
            }
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
