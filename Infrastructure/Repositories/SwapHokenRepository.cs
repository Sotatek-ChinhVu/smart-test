using Domain.Models.SwapHoken;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Infrastructure.Services;

namespace Infrastructure.Repositories
{
    public class SwapHokenRepository : RepositoryBase, ISwapHokenRepository
    {
        public SwapHokenRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public long CountOdrInf(int hpId, long ptId, int hokenPid, int startDate, int endDate)
        {
            return NoTrackingDataContext.OdrInfs.Count((x) =>
                            x.HpId == hpId &&
                            x.PtId == ptId &&
                            x.HokenPid == hokenPid &&
                            x.SinDate >= startDate &&
                            x.SinDate <= endDate &&
                            x.IsDeleted == 0);
        }

        public List<int> GetListSeikyuYms(int hpId, long ptId, int hokenPid, int startDate, int endDate)
        {
            var listRaiin = NoTrackingDataContext.RaiinInfs.Where(x =>
                        x.HpId == hpId &&
                        x.PtId == ptId &&
                        x.HokenPid == hokenPid &&
                        x.Status >= RaiinState.TempSave &&
                        x.IsDeleted == DeleteTypes.None &&
                        x.SinDate >= startDate &&
                        x.SinDate <= endDate);

            return listRaiin.GroupBy(x => x.SinDate / 100).Select(x => x.Key).ToList();
        }

        public List<int> GetSeikyuYmsInPendingSeikyu(int hpId, long ptId, List<int> sinYms, int hokenId)
        {
            return NoTrackingDataContext.ReceSeikyus.Where(p => p.HpId == hpId &&
                                                        p.PtId == ptId &&
                                                        p.IsDeleted == DeleteTypes.None &&
                                                        sinYms.Contains(p.SinYm) &&
                                                        p.SeikyuYm != 999999 &&
                                                        p.HokenId == hokenId)
                                                 .Select(p => p.SeikyuYm).ToList();
        }

        public bool SwapHokenParttern(int hpId, long PtId, int OldHokenPid, int NewHokenPid, int StartDate, int EndDate, int userId)
        {
            var updateDate = CIUtil.GetJapanDateTimeNow();
            var updateId = userId;

            #region UpdateHokenPatternInRaiin
            var raiinInfList = TrackingDataContext.RaiinInfs.Where((x) =>
                x.HpId == hpId &&
                x.PtId == PtId &&
                x.HokenPid == OldHokenPid &&
                x.Status >= RaiinState.TempSave &&
                x.SinDate >= StartDate &&
                x.SinDate <= EndDate &&
                x.IsDeleted == DeleteTypes.None).ToList();

            raiinInfList.ForEach(x =>
            {
                x.HokenPid = NewHokenPid;
                x.UpdateDate = updateDate;
                x.UpdateId = updateId;
            });
            #endregion UpdateHokenPatternInRaiin

            #region UpdateHokenPatternInOdrInf
            var odrInfList = TrackingDataContext.OdrInfs.Where((x) =>
                    x.HpId == hpId &&
                    x.PtId == PtId &&
                    x.SinDate >= StartDate &&
                    x.SinDate <= EndDate &&
                    x.HokenPid == OldHokenPid).ToList();

            odrInfList.ForEach((x) =>
            {
                x.HokenPid = NewHokenPid;
                x.UpdateDate = updateDate;
                x.UpdateId = updateId;
            });
            #endregion 

            return TrackingDataContext.SaveChanges() > 0;
        }

        public bool ExistRaiinInfUsedOldHokenId(int hpId, long ptId, List<int> sinYms, int oldHokenPId)
        {
            return NoTrackingDataContext.RaiinInfs.Any(p => p.HpId == hpId &&
                                                         p.PtId == ptId &&
                                                         sinYms.Contains(p.SinDate / 100) &&
                                                         p.HokenPid == oldHokenPId &&
                                                         p.Status >= RaiinState.TempSave &&
                                                         p.IsDeleted == DeleteTypes.None);
        }

        public bool UpdateReceSeikyu(int hpId, long ptId, List<int> seiKyuYms, int oldHokenId, int newHokenId, int userId)
        {
            var receSeiKyus = TrackingDataContext.ReceSeikyus.Where(p => p.HpId == hpId && p.PtId == ptId && seiKyuYms.Contains(p.SeikyuYm)).ToList();
            if (!receSeiKyus.Any())
                return true;
            foreach (var receSeiKyu in receSeiKyus)
            {
                if (oldHokenId != newHokenId && receSeiKyu.HokenId == newHokenId)
                    receSeiKyu.IsDeleted = DeleteTypes.Deleted;
                receSeiKyu.HokenId = newHokenId;
                receSeiKyu.UpdateDate = DateTime.Now;
                receSeiKyu.UpdateId = userId;
            }
            return TrackingDataContext.SaveChanges() > 0;
        }

        public bool IsPtHokenPatternUsed(int hpId, long ptId, int hokenPid)
        {
            var countOdr = NoTrackingDataContext.OdrInfs
                .Count(
                    x => x.HpId == hpId &&
                    x.PtId == ptId &&
                    x.HokenPid == hokenPid &&
                    x.IsDeleted == 0);
            return countOdr != 0;
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}