using Domain.Constant;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.SyojyoSyoki.Model;

namespace Reporting.SyojyoSyoki.DB
{
    public class CoSyojyoSyokiFinder : RepositoryBase, ICoSyojyoSyokiFinder
    {
        public CoSyojyoSyokiFinder(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }

        public List<CoSyojyoSyokiModel> FindSyoukiInf(int hpId, long ptId, int seikyuYm, int sinYm, int hokenId)
        {

            var ptInfs = NoTrackingDataContext.PtInfs
                .Where(p =>
                    p.HpId == hpId &&
                    p.PtId == (ptId > 0 ? ptId : p.PtId) &&
                    p.IsDelete == DeleteTypes.None);

            var receInfs =
                NoTrackingDataContext.ReceInfs.Where(p =>
                    p.HpId == hpId &&
                    p.PtId == (ptId > 0 ? ptId : p.PtId) &&
                    p.SeikyuYm == seikyuYm &&
                    p.SinYm == (sinYm > 0 ? sinYm : p.SinYm) &&
                    p.HokenId == (hokenId > 0 ? hokenId : p.HokenId)
                );

            var ptHokenInfs =
                NoTrackingDataContext.PtHokenInfs.Where(p =>
                    p.HpId == hpId &&
                    p.PtId == (ptId > 0 ? ptId : p.PtId) &&
                    p.IsDeleted == DeleteStatus.None
                );
            var ptKohis =
                NoTrackingDataContext.PtKohis.Where(p =>
                p.HpId == hpId &&

                p.IsDeleted == DeleteStatus.None
                );
            var hpInfs =
                NoTrackingDataContext.HpInfs.Where(p => p.HpId == hpId).OrderByDescending(p => p.StartDate);

            var receInfDatas = (
                        from receInf in receInfs
                        join ptInf in ptInfs on
                            new { receInf.HpId, receInf.PtId } equals
                            new { ptInf.HpId, ptInf.PtId }
                        join ptHokenInf in ptHokenInfs on
                            new { receInf.HpId, receInf.PtId, receInf.HokenId } equals
                            new { ptHokenInf.HpId, ptHokenInf.PtId, ptHokenInf.HokenId } into ptHokenInfJoins
                        from ptHokenInfJoin in ptHokenInfJoins.DefaultIfEmpty()
                        join ptKohi1 in ptKohis on
                            new { receInf.HpId, receInf.PtId, HokenId = receInf.Kohi1Id } equals
                            new { ptKohi1.HpId, ptKohi1.PtId, ptKohi1.HokenId } into ptKohi1Joins
                        from ptKohi1Join in ptKohi1Joins.DefaultIfEmpty()
                        join ptKohi2 in ptKohis on
                            new { receInf.HpId, receInf.PtId, HokenId = receInf.Kohi1Id } equals
                            new { ptKohi2.HpId, ptKohi2.PtId, ptKohi2.HokenId } into ptKohi2Joins
                        from ptKohi2Join in ptKohi2Joins.DefaultIfEmpty()
                        join ptKohi3 in ptKohis on
                            new { receInf.HpId, receInf.PtId, HokenId = receInf.Kohi1Id } equals
                            new { ptKohi3.HpId, ptKohi3.PtId, ptKohi3.HokenId } into ptKohi3Joins
                        from ptKohi3Join in ptKohi3Joins.DefaultIfEmpty()
                        join ptKohi4 in ptKohis on
                            new { receInf.HpId, receInf.PtId, HokenId = receInf.Kohi1Id } equals
                            new { ptKohi4.HpId, ptKohi4.PtId, ptKohi4.HokenId } into ptKohi4Joins
                        from ptKohi4Join in ptKohi4Joins.DefaultIfEmpty()
                        orderby
                            receInf.SinYm, ptInf.PtNum
                        select new
                        {
                            ptInf,
                            receInf,
                            ptHokenInf = ptHokenInfJoin,
                            ptKohi1 = ptKohi1Join,
                            ptKohi2 = ptKohi2Join,
                            ptKohi3 = ptKohi3Join,
                            ptKohi4 = ptKohi4Join
                        }
                    ).ToList();

            var syoukiInfs =
                NoTrackingDataContext.SyoukiInfs.Where(p =>
                    p.HpId == hpId &&
                    p.PtId == (ptId > 0 ? ptId : p.PtId) &&
                    p.SinYm == (sinYm > 0 ? sinYm : p.SinYm) &&
                    p.HokenId == (hokenId > 0 ? hokenId : p.HokenId) &&
                    p.IsDeleted == DeleteStatus.None);

            var syoukiKbnMsts =
                NoTrackingDataContext.SyoukiKbnMsts.AsQueryable();
            var syoukiJoins = (
                from receInf in receInfs
                join syoukiInf in syoukiInfs on
                    new { receInf.HpId, receInf.PtId, receInf.SinYm, receInf.HokenId } equals
                    new { syoukiInf.HpId, syoukiInf.PtId, syoukiInf.SinYm, syoukiInf.HokenId }
                join syoukiKbnMst in syoukiKbnMsts on
                    new { syoukiInf.SyoukiKbn } equals
                    new { syoukiKbnMst.SyoukiKbn } into syoukiInfJoins
                from syoukiJoin in syoukiInfJoins.DefaultIfEmpty()
                where (syoukiJoin == null ||
                    (syoukiJoin.StartYm <= receInf.SinYm && syoukiJoin.EndYm >= receInf.SinYm))
                select new
                {
                    syoukiInf,
                    syoukiKbnMst = syoukiJoin
                }
                ).ToList();

            List<CoSyojyoSyokiModel> results = new List<CoSyojyoSyokiModel>();

            List<CoSyoukiInfModel> syoukiInfModels = new List<CoSyoukiInfModel>();

            receInfDatas?.ForEach(receInfData =>
            {
                syoukiInfModels =
                    syoukiJoins
                        .Where(p =>
                            p.syoukiInf.PtId == receInfData.receInf.PtId &&
                            p.syoukiInf.SinYm == receInfData.receInf.SinYm &&
                            p.syoukiInf.HokenId == receInfData.receInf.HokenId)
                        .Select(p => new CoSyoukiInfModel(p.syoukiInf, p.syoukiKbnMst))
                        .ToList();

                if (syoukiInfModels.Any())
                {
                    HpInf hpInf = hpInfs.FirstOrDefault(p => p.StartDate <= receInfData.receInf.SinYm * 100 + 31);

                    results.Add(
                            new CoSyojyoSyokiModel(
                                hpInf, syoukiInfModels,
                                receInfData.receInf, receInfData.ptInf,
                                receInfData.ptHokenInf, receInfData.ptKohi1, receInfData.ptKohi2, receInfData.ptKohi3, receInfData.ptKohi4)
                        );
                }
            }
            );

            return results;
        }
    }
}
