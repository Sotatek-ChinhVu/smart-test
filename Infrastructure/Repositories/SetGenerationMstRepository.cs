using Domain.Models.SetGenerationMst;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace Infrastructure.Repositories
{
    public class SetGenerationMstRepository : RepositoryBase, ISetGenerationMstRepository
    {
        private readonly IMemoryCache _memoryCache;
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
            int k = 1;
            for (int i = 0; i < setGenerationMsts.Count; i++)
            {
                if (i == 0)
                {
                    result.Add(new SetSendaiGenerationModel(hpId, setGenerationMsts[i].GenerationId, setGenerationMsts[i].StartDate, convertDateDisplay(setGenerationMsts[i].StartDate), 0, convertDateDisplay(0), i));
                }
                else
                {
                    DateTime endTimeDate = CIUtil.IntToDate(setGenerationMsts[i - 1].StartDate);
                    endTimeDate = endTimeDate == DateTime.MinValue ? DateTime.MinValue : endTimeDate.AddDays(-1);
                    var endDateInt = CIUtil.DateTimeToInt(endTimeDate);
                    result.Add(new SetSendaiGenerationModel(hpId, setGenerationMsts[i].GenerationId, setGenerationMsts[i].StartDate, convertDateDisplay(setGenerationMsts[i].StartDate), endDateInt, convertDateDisplay(endDateInt), i));
                }
                k++;
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

        public bool AddSetSendaiGeneration(int userId, int hpId, int startDate)
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
            TrackingDataContext.SetGenerationMsts.Add(itemAdd);
            var checkAdd = TrackingDataContext.SaveChanges();
            if (checkAdd == 0)
            {
                return false;
            }
            else
            {
                // Clone Generation
                var itemAddGet = TrackingDataContext.Entry(itemAdd).Entity;
                if (itemNewest != null && itemAddGet != null)
                {
                    CloneGeneration(itemAddGet.GenerationId, itemNewest.GenerationId, hpId, userId);
                }
            }
            return true;
        }

        public void CloneGeneration(int targetGenerationId, int sourceGenerationId, int hpId, int userId)
        {
            var setMstsBackuped = TrackingDataContext.SetMsts.Where(x =>
                x.HpId == hpId &&
                x.GenerationId == sourceGenerationId).ToList();
            var setMstDict = new List<int>();
            for (int i = 0, len = setMstsBackuped.Count; i < len; i++)
            {
                if (!setMstDict.Contains(setMstsBackuped[i].SetCd))
                {
                    setMstDict.Add(setMstsBackuped[i].SetCd);
                }
            }

            var setKbnMstSource = TrackingDataContext.SetKbnMsts.Where(x =>
                x.HpId == hpId &&
                x.GenerationId == sourceGenerationId).ToList();

            var setByomeisSource = TrackingDataContext.SetByomei.Where(setByomei =>
                setByomei.HpId == hpId && setMstDict.Contains(setByomei.SetCd))
                .ToList();

            var setKarteInfsSource = TrackingDataContext.SetKarteInf.Where(setKarteInf =>
                setKarteInf.HpId == hpId && setMstDict.Contains(setKarteInf.SetCd))
                .ToList();

            var setKarteImgInfsSource = TrackingDataContext.SetKarteImgInf.Where(setKarteImgInf =>
                setKarteImgInf.HpId == hpId && setMstDict.Contains(setKarteImgInf.SetCd))
                .ToList();

            var setOdrInfsSource = TrackingDataContext.SetOdrInf.Where(setOdrInf =>
                setOdrInf.HpId == hpId && setMstDict.Contains(setOdrInf.SetCd))
                .ToList();

            var setOdrInfDetailsSource = TrackingDataContext.SetOdrInfDetail.Where(setOdrInfDetail =>
                setOdrInfDetail.HpId == hpId && setMstDict.Contains(setOdrInfDetail.SetCd))
                .ToList();

            var setOdrInfCmtSource = TrackingDataContext.SetOdrInfCmt.Where(setOdrInfCmt =>
                setOdrInfCmt.HpId == hpId && setMstDict.Contains(setOdrInfCmt.SetCd))
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
            if(setMstsBackuped.Any())
            {
                TrackingDataContext.SetMsts.AddRange(setMstsBackuped);
            }


            //setKbnMst
            setKbnMstSource.ForEach(setKbnMst =>
            {
                setKbnMst.GenerationId = targetGenerationId;
                setKbnMst.CreateDate = DateTime.Now;
                setKbnMst.CreateId = Session.UserID;
                setKbnMst.CreateMachine = computerName;
                setKbnMst.UpdateDate = DateTime.Now;
                setKbnMst.UpdateId = Session.UserID;
                setKbnMst.UpdateMachine = computerName;
            });
            if(setKbnMstSource.Any())
            {
                TrackingDataContext.SetKbnMsts.AddRange(setKbnMstSource);
            }

            //setByomeis
            setByomeisSource.ForEach(x =>
            {
                x.CreateDate = DateTime.Now;
                x.CreateId = Session.UserID;
                x.CreateMachine = computerName;
                x.UpdateDate = DateTime.Now;
                x.UpdateId = Session.UserID;
                x.UpdateMachine = computerName;
            });
            if(setByomeisSource.Any())
            {
                TrackingDataContext.SetByomei.AddRange(setByomeisSource);
            }

            //setKarteInf
            setKarteInfsSource.ForEach(x =>
            {
                x.CreateDate = DateTime.Now;
                x.CreateId = Session.UserID;
                x.CreateMachine = computerName;
                x.UpdateDate = DateTime.Now;
                x.UpdateId = Session.UserID;
                x.UpdateMachine = computerName;
            });
            if(setKarteInfsSource.Any())
            {
                TrackingDataContext.SetKarteInf.AddRange(setKarteInfsSource);
            }

            //setKarteImgInf
            setKarteImgInfsSource.ForEach(x =>
            {
                x.Id = 0;
            });
            if (setKarteImgInfsSource.Any())
            {
                TrackingDataContext.SetKarteImgInf.AddRange(setKarteImgInfsSource);
            }

            //setOdrInf
            setOdrInfsSource.ForEach((x) =>
            {
                x.Id = 0;
                x.CreateDate = DateTime.Now;
                x.CreateId = Session.UserID;
                x.CreateMachine = computerName;
                x.UpdateDate = DateTime.Now;
                x.UpdateId = Session.UserID;
                x.UpdateMachine = computerName;
            });
            if(setOdrInfsSource.Any())
            {
                TrackingDataContext.SetOdrInf.AddRange(setOdrInfsSource);
            }

            //setOdrInfDetail
            setOdrInfDetailsSource.ForEach((x) =>
            {
                
            });
            if(setOdrInfDetailsSource.Any())
            {
                TrackingDataContext.SetOdrInfDetail.AddRange(setOdrInfDetailsSource);
            }

            //setOdrInfCmt
            setOdrInfCmtSource.ForEach((x) => { 
            
            });
            if(setOdrInfCmtSource.Any())
            {
                TrackingDataContext.SetOdrInfCmt.AddRange(setOdrInfCmtSource);
            }

            if(countData > 0)
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
