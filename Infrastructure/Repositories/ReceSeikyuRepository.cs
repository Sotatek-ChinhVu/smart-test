using Domain.Models.ReceSeikyu;
using Entity.Tenant;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class ReceSeikyuRepository : RepositoryBase, IReceSeikyuRepository
    {
        public ReceSeikyuRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public List<ReceSeikyuModel> GetListReceSeikyModel(int hpId ,int sinDate, int sinYm)
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

            //result = query.Select(x=> new ReceSeikyuModel(x.ReceSeikyuPending.PtInfo.HpId,
            //                                              x.ReceSeikyuPending.PtInfo.PtId,
            //                                              x.ReceSeikyuPending.PtInfo.Name,
            //                                              sinYm,
            //                                              sinYm,

                                                          
            //                                              ))

            //result = query.AsEnumerable().Select(u => new ReceSeikyuModel(sinDate,
            //                                                              sinYm,
            //                                                              u.ReceSeikyuPending.PtInfo,
            //                                                              u.ReceSeikyuPending.ReceSeikyu,
            //                                                              u.ReceSeikyuPending.PtHokenInfItem ?? new PtHokenInf(),
            //                                                              u.recedenHenjiyuuList.Select(p => new RecedenHenJiyuuModel(p.RecedenHenJiyuu, p.PtHokenInfItem)).ToList())
            //{
            //    OriginSeikyuYm = u.ReceSeikyuPending.ReceSeikyu.SeikyuYm,
            //    OriginSinYm = u.ReceSeikyuPending.ReceSeikyu.SinYm,
            //    IsChecked = u.ReceSeikyuPending.ReceSeikyu.SeikyuYm != 999999
            //}
            //).OrderByDescending(u => u.SeikyuYm).ThenBy(u => u.SinYm).ThenBy(u => u.PtInf.PtNum).ToList();

            return result;
        }
    }
}
