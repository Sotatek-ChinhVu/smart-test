using Domain.Models.ReceSeikyu;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class ReceSeikyuRepository : RepositoryBase, IReceSeikyuRepository
    {
        public ReceSeikyuRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public List<ReceSeikyuModel> GetListReceSeikyModel(int hpId, int sinDate, int sinYm, bool isIncludingUnConfirmed, long ptNumSearch, bool noFilter, bool isFilterMonthlyDelay, bool isFilterReturn, bool isFilterOnlineReturn)
        {
            var ptInfo = NoTrackingDataContext.PtInfs.Where(u => u.HpId == hpId &&
                                                                                u.IsDelete == 0);

            var listReceSeikyu = NoTrackingDataContext.ReceSeikyus.Where(u => u.HpId == hpId &&
                                                                                       u.IsDeleted == 0);

            var ptHoken = NoTrackingDataContext.PtHokenInfs.Where(u => u.HpId == hpId && u.IsDeleted == 0);


            var listRecedenHenjiyuu = NoTrackingDataContext.RecedenHenJiyuus.Where(u => u.HpId == hpId &&
                                                                                                 u.IsDeleted == 0);

            var recedenHenjiyuuInfo = from recedenHenjiyuu in listRecedenHenjiyuu
                                      join ptHokenInf in ptHoken on
                                           new { recedenHenjiyuu.PtId, recedenHenjiyuu.HokenId } equals
                                           new { ptHokenInf.PtId, ptHokenInf.HokenId } into lj
                                      from item in lj.DefaultIfEmpty()
                                      select new
                                      {
                                          RecedenHenJiyuu = recedenHenjiyuu,
                                          PtHokenInfItem = item
                                      };

            var receSeikyuInf = from receSeikyu in listReceSeikyu
                                join ptHokenInf in ptHoken on
                                  new { receSeikyu.PtId, receSeikyu.HokenId } equals
                                  new { ptHokenInf.PtId, ptHokenInf.HokenId } into ptHokenList
                                from item in ptHokenList.DefaultIfEmpty()
                                join ptInf in ptInfo on receSeikyu.PtId equals ptInf.PtId
                                where receSeikyu.IsDeleted == DeleteTypes.None
                                      && !(!isIncludingUnConfirmed && receSeikyu.SeikyuYm == 999999)
                                      && !(sinYm > 0 && receSeikyu.SeikyuYm != 999999 && receSeikyu.SeikyuYm != sinYm)

                                      && (ptNumSearch == 0 || ptInf.PtNum == ptNumSearch)
                                      && (noFilter ||
                                            (isFilterMonthlyDelay && receSeikyu.SeikyuKbn == 1) ||
                                            (isFilterReturn && receSeikyu.SeikyuKbn == 2) ||
                                            (isFilterOnlineReturn && receSeikyu.SeikyuKbn == 3)
                                          )
                                select new
                                {
                                    PtInfo = ptInf,
                                    ReceSeikyu = receSeikyu,
                                    PtHokenInfItem = item ?? new PtHokenInf()
                                };

            var query = from receSeikyu in receSeikyuInf
                        select new
                        {
                            recedenHenjiyuuList = recedenHenjiyuuInfo.Where(x => x.RecedenHenJiyuu.PtId == receSeikyu.ReceSeikyu.PtId
                                                                                && x.RecedenHenJiyuu.SinYm == receSeikyu.ReceSeikyu.SinYm
                                                                                && x.RecedenHenJiyuu.HokenId == receSeikyu.ReceSeikyu.HokenId).AsEnumerable(),
                            ReceSeikyuPending = receSeikyu,
                        };

            return query.AsEnumerable().Select(x => new ReceSeikyuModel(sinDate,
                                                          x.ReceSeikyuPending.PtInfo.HpId,
                                                          x.ReceSeikyuPending.PtInfo.PtId,
                                                          x.ReceSeikyuPending.PtInfo.Name ?? string.Empty,
                                                          sinYm,
                                                          sinYm,
                                                          x.ReceSeikyuPending.PtHokenInfItem.HokenId,
                                                          x.ReceSeikyuPending.PtHokenInfItem.HokensyaNo ?? string.Empty,
                                                          x.ReceSeikyuPending.ReceSeikyu.SeqNo,
                                                          x.ReceSeikyuPending.ReceSeikyu.SeikyuYm,
                                                          x.ReceSeikyuPending.ReceSeikyu.SeikyuYm.ToString(),
                                                          x.ReceSeikyuPending.ReceSeikyu.SeikyuKbn,
                                                          x.ReceSeikyuPending.ReceSeikyu.PreHokenId,
                                                          x.ReceSeikyuPending.ReceSeikyu.Cmt ?? string.Empty,
                                                          false,
                                                          x.ReceSeikyuPending.PtInfo.PtNum,
                                                          x.ReceSeikyuPending.PtHokenInfItem.HokenKbn,
                                                          x.ReceSeikyuPending.PtHokenInfItem.Houbetu ?? string.Empty,
                                                          x.ReceSeikyuPending.PtHokenInfItem.StartDate,
                                                          x.ReceSeikyuPending.PtHokenInfItem.EndDate,
                                                          false,
                                                          sinYm,
                                                          sinYm,
                                                          x.recedenHenjiyuuList.Select(m => new RecedenHenJiyuuModel(hpId,
                                                                                                                    m.RecedenHenJiyuu.PtId,
                                                                                                                    m.PtHokenInfItem.HokenId,
                                                                                                                    sinYm,
                                                                                                                    m.RecedenHenJiyuu.SeqNo,
                                                                                                                    m.RecedenHenJiyuu.HenreiJiyuuCd ?? string.Empty,
                                                                                                                    m.RecedenHenJiyuu.HenreiJiyuu ?? string.Empty,
                                                                                                                    m.RecedenHenJiyuu.Hosoku ?? string.Empty,
                                                                                                                    0,
                                                                                                                    m.PtHokenInfItem.HokenKbn,
                                                                                                                    m.PtHokenInfItem.Houbetu ?? string.Empty,
                                                                                                                    m.PtHokenInfItem.SikakuDate,
                                                                                                                    m.PtHokenInfItem.EndDate,
                                                                                                                    m.PtHokenInfItem.HokensyaNo ?? string.Empty)).ToList()
                                                          )).OrderByDescending(o => o.SeikyuKbn).ThenBy(u => u.SinYm).ThenBy(i => i.PtNum).ToList();
        }

        public List<ReceSeikyuModel> GetListReceSeikyModel(int hpId, int seikyuYm, List<long> ptIdList)
        {
            List<ReceSeikyuModel> result = new();
            var ptInfList = NoTrackingDataContext.PtInfs.Where(item => item.HpId == hpId
                                                                    && item.IsDelete == DeleteTypes.None
                                                                    && (ptIdList.Count <= 0 || ptIdList.Contains(item.PtId)))
                                                     .ToList();

            ptIdList = ptInfList.Select(item => item.PtId).Distinct().ToList();
            var receSeikyus = NoTrackingDataContext.ReceSeikyus.Where(item => item.HpId == hpId
                                                                              && item.SeikyuYm == seikyuYm
                                                                              && item.IsDeleted == DeleteTypes.None
                                                                              && ptIdList.Contains(item.PtId))
                                                                .ToList();

            foreach (var ptInf in ptInfList)
            {
                var receSeikyu = receSeikyus.FirstOrDefault(item => item.PtId == ptInf.PtId);
                if (receSeikyu == null)
                {
                    continue;
                }
                result.Add(new ReceSeikyuModel(
                               ptInf.PtId,
                               receSeikyu?.SinYm ?? 0,
                               receSeikyu?.HokenId ?? 0,
                               ptInf.PtNum,
                               receSeikyu?.SeikyuKbn ?? 0
                          ));
            }
            return result;
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
