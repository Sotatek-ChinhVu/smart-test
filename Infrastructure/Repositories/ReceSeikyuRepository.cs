using Domain.Models.ReceSeikyu;
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
            List<ReceSeikyuModel> result = new List<ReceSeikyuModel>();

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
                                           new { ptHokenInf.PtId, ptHokenInf.HokenId } into ptHokenList
                                      select new
                                      {
                                          RecedenHenJiyuu = recedenHenjiyuu,
                                          PtHokenInfItem = ptHokenList.FirstOrDefault(),
                                      };
            var receSeikyuInf = from receSeikyu in listReceSeikyu
                                join ptHokenInf in ptHoken on
                                  new { receSeikyu.PtId, receSeikyu.HokenId } equals
                                  new { ptHokenInf.PtId, ptHokenInf.HokenId } into ptHokenList
                                join ptInf in ptInfo on receSeikyu.PtId equals ptInf.PtId
                                select new
                                {
                                    PtInfo = ptInf,
                                    ReceSeikyu = receSeikyu,
                                    PtHokenInfItem = ptHokenList.FirstOrDefault()
                                };
            var query = from receSeikyu in receSeikyuInf
                        join recedenHenjiyuu in recedenHenjiyuuInfo on
                             new { receSeikyu.ReceSeikyu.PtId, receSeikyu.ReceSeikyu.SinYm, receSeikyu.ReceSeikyu.HokenId } equals
                             new { recedenHenjiyuu.RecedenHenJiyuu.PtId, recedenHenjiyuu.RecedenHenJiyuu.SinYm, recedenHenjiyuu.RecedenHenJiyuu.HokenId } into recedenHenjiyuuList
                        select new
                        {
                            recedenHenjiyuuList = recedenHenjiyuuList,
                            ReceSeikyuPending = receSeikyu,
                        };

            result = query.Select(x => new ReceSeikyuModel(sinDate,
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
                                                          )).OrderByDescending(o => o.SeikyuKbn).ThenBy(u=>u.SinYm).ThenBy(i=>i.PtNum).ToList();
            return result;
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
