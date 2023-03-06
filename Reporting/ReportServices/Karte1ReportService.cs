using Infrastructure.Interfaces;
using Reporting.Interface;
using Reporting.Karte1.DB;
using Reporting.Karte1.Model;
using Reporting.Mappers;
using Reporting.NameLabel.DB;
using Reporting.NameLabel.Models;
using CoPtInfModel = Reporting.Karte1.Model.CoPtInfModel;

namespace Reporting.ReportServices
{
    public class Karte1ReportService : IReportService
    {
        private readonly ITenantProvider _tenantProvider;

        public Karte1ReportService(ITenantProvider tenantProvider)
        {
            _tenantProvider = tenantProvider;
        }

        public Karte1Mapper GetKarte1ReportingData(int hpId, long ptId, int sinDate, int hokenPid, bool tenkiByomei, bool syuByomei)
        {
            using (var noTrackingDataContext = _tenantProvider.GetNoTrackingDataContext())
            {
                var finder = new CoKarte1Finder(noTrackingDataContext);
                // 白紙印刷の場合、データ取得しない
                if (ptId == 0) return null;

                // 患者情報
                CoPtInfModel ptInf = finder.FindPtInf(ptId, sinDate);

                // 病名情報
                List<CoPtByomeiModel> ptByomeis = finder.FindPtByomei(ptId, hokenPid, tenkiByomei);
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

        public CoNameLabelModel GetNameLabelReportingData(long ptId, string kanjiName, int sinDate)
        {
            using (var noTrackingDataContext = _tenantProvider.GetNoTrackingDataContext())
            {
                var finder = new CoNameLabelFinder(noTrackingDataContext);

                // 患者情報
                NameLabel.Models.CoPtInfModel ptInf = finder.FindPtInf(ptId);

                return new CoNameLabelModel(ptInf, kanjiName, sinDate);
            }
        }
    }
}
