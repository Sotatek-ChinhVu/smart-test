using Domain.Models.SwapHoken;
using Helper.Common;
using Helper.Constants;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class SwapHokenRepository : ISwapHokenRepository
    {
        private readonly TenantDataContext _tenantDataContext;

        public SwapHokenRepository(TenantDataContext tenantDataContext)
        {
            _tenantDataContext = tenantDataContext;
        }

        public long CountOdrInf(int hpId,long ptId, int hokenPid, int startDate, int endDate)
        {
            return _tenantDataContext.OdrInfs.Count((x) =>
                            x.HpId == hpId &&
                            x.PtId == ptId &&
                            x.HokenPid == hokenPid &&
                            x.SinDate >= startDate &&
                            x.SinDate <= endDate &&
                            x.IsDeleted == 0);

        }

        public List<int> GetListSeikyuYms(int hpId, long ptId, int hokenPid, int startDate, int endDate)
        {
            var listRaiin = _tenantDataContext.RaiinInfs.Where(x =>
                        x.HpId == hpId &&
                        x.PtId == ptId &&
                        x.HokenPid == hokenPid &&
                        x.Status >= RaiinState.TempSave &&
                        x.IsDeleted == DeleteTypes.None &&
                        x.SinDate >= startDate &&
                        x.SinDate <= endDate);

            return listRaiin.GroupBy(x => x.SinDate / 100).Select(x => x.Key).ToList();
        }

        public List<int> GetSeikyuYmsInPendingSeikyu(int hpId,long ptId, List<int> sinYms, int hokenId)
        {
            return _tenantDataContext.ReceSeikyus.Where(p => p.HpId == hpId &&
                                                        p.PtId == ptId &&
                                                        p.IsDeleted == DeleteTypes.None &&
                                                        sinYms.Contains(p.SinYm) &&
                                                        p.SeikyuYm != 999999 &&
                                                        p.HokenId == hokenId)
                                                 .Select(p => p.SeikyuYm).ToList();
        }

        public bool SwapHokenParttern(int hpId,long PtId, int OldHokenPid, int NewHokenPid, int StartDate, int EndDate)
        {
            var updateDate = DateTime.Now;
            var updateMachine = TempIdentity.ComputerName;
            var updateId = TempIdentity.UserId;

            #region UpdateHokenPatternInRaiin
            var raiinInfs = _tenantDataContext.RaiinInfs.Where((x) =>
                x.HpId == hpId &&
                x.PtId == PtId &&
                x.HokenPid == OldHokenPid &&
                x.Status >= RaiinState.TempSave &&
                x.SinDate >= StartDate &&
                x.SinDate <= EndDate &&
                x.IsDeleted == DeleteTypes.None);
            var raiinInfList = raiinInfs.Select(p => p).ToList();
            foreach (var raiinInf in raiinInfList)
            {
                raiinInf.HokenPid = NewHokenPid;
                raiinInf.UpdateDate = updateDate;
                raiinInf.UpdateMachine = updateMachine;
                raiinInf.UpdateId = updateId;
            }
            #endregion

            #region UpdateHokenPatternInOdrInf
            var odrInfs = _tenantDataContext.OdrInfs.Where((x) =>
                    x.HpId == hpId &&
                    x.PtId == PtId &&
                    x.SinDate >= StartDate &&
                    x.SinDate <= EndDate &&
                    x.HokenPid == OldHokenPid);
            var odrInfList = odrInfs.Select((p) => p).ToList();
            odrInfList.ForEach((x) =>
            {
                x.HokenPid = NewHokenPid;
                x.UpdateDate = updateDate;
                x.UpdateMachine = updateMachine;
                x.UpdateId = updateId;
            });
            #endregion

            return _tenantDataContext.SaveChanges() > 0;
        }

        public bool ExistRaiinInfUsedOldHokenId(int hpId,long ptId, List<int> sinYms, int oldHokenPId)
        {
            return _tenantDataContext.RaiinInfs.Any(p => p.HpId == hpId &&
                                                                              p.PtId == ptId &&
                                                                              sinYms.Contains(p.SinDate / 100) &&
                                                                              p.HokenPid == oldHokenPId &&
                                                                              p.Status >= RaiinState.TempSave &&
                                                                              p.IsDeleted == DeleteTypes.None);
        }

        public bool UpdateReceSeikyu(int hpId,long ptId, List<int> seiKyuYms, int oldHokenId, int newHokenId)
        {
            var receSeiKyus = _tenantDataContext.ReceSeikyus.Where(p => p.HpId == hpId && p.PtId == ptId && seiKyuYms.Contains(p.SeikyuYm));
            foreach (var receSeiKyu in receSeiKyus)
            {
                if (oldHokenId != newHokenId && receSeiKyu.HokenId == newHokenId)
                {
                    receSeiKyu.IsDeleted = DeleteTypes.Deleted;
                }
                receSeiKyu.HokenId = newHokenId;
                receSeiKyu.UpdateDate = DateTime.Now;
                receSeiKyu.UpdateId = TempIdentity.UserId;
                receSeiKyu.UpdateMachine = TempIdentity.ComputerName;
            }
            _tenantDataContext.SaveChanges();
            return true;
        }
    }
}
