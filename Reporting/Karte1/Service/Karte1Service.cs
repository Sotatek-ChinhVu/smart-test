using Infrastructure.Interfaces;
using Reporting.Karte1.DB;
using Reporting.Karte1.Mapper;
using Reporting.Karte1.Model;

namespace Reporting.Karte1.Service;

public class Karte1Service : IKarte1Service
{
    private readonly ITenantProvider _tenantProvider;
    public Karte1Service(ITenantProvider tenantProvider)
    {
        _tenantProvider = tenantProvider;
    }

    public Karte1Mapper GetKarte1ReportingData(int hpId, long ptId, int sinDate, int hokenPid, bool tenkiByomei, bool syuByomei)
    {
        using (var noTrackingDataContext = _tenantProvider.GetNoTrackingDataContext())
        {
            var finder = new CoKarte1Finder(noTrackingDataContext);
            // 白紙印刷の場合、データ取得しない
            if (ptId == 0) return new();

            // 患者情報
            CoPtInfModel ptInf = finder.FindPtInf(ptId, sinDate);

            // 病名情報
            List<CoPtByomeiModel> ptByomeis = finder.FindPtByomei(ptId, hokenPid, tenkiByomei);

            //ToDo: DuongLe need to update entity to uncomment below code
            //if (syuByomei)
            //{
            //    foreach (var item in ptByomeis)
            //    {
            //        if (item.SyobyoKbn == 1)
            //        {
            //            item.Byomei = "（主）" + item.Byomei;
            //        }
            //    }
            //}

            // 患者保険情報
            CoPtHokenInfModel ptHokenInf = finder.FindPtHokenInf(ptId, hokenPid, sinDate);

            CoKarte1Model coKarte1Model = new CoKarte1Model(ptInf, ptByomeis, ptHokenInf);

            return new Karte1Mapper(coKarte1Model);
        }
    }
}
