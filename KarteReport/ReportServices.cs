using Infrastructure.Interfaces;
using KarteReport.Interface;
using KarteReport.Karte1.DB;
using KarteReport.Karte1.Model;
using KarteReport.NameLabel.DB;
using KarteReport.NameLabel.Models;

namespace KarteReport
{
    public class ReportServices : IReportServices
    {
        private readonly ITenantProvider _tenantProvider;

        public ReportServices(ITenantProvider tenantProvider)
        {
            _tenantProvider = tenantProvider;
        }

        public CoKarte1Model GetKarte1ReportingData(int hpId, long ptId, int sinDate, int hokenPid, bool tenkiByomei, bool syuByomei)
        {
            using (var noTrackingDataContext = _tenantProvider.GetNoTrackingDataContext())
            {
                var finder = new CoKarte1Finder(noTrackingDataContext);
                // 白紙印刷の場合、データ取得しない
                if (ptId == 0) return null;

                // 患者情報
                Karte1.Model.CoPtInfModel ptInf = finder.FindPtInf(ptId, sinDate);

                // 病名情報
                List<CoPtByomeiModel> ptByomeis = finder.FindPtByomei(ptId, hokenPid, tenkiByomei);
                if (syuByomei)
                {
                    foreach (var item in ptByomeis)
                    {
                        if (item.SyobyoKbn == 1)
                        {
                            item.Byomei = "（主）" + item.Byomei;
                        }
                    }
                }

                // 患者保険情報
                CoPtHokenInfModel ptHokenInf = finder.FindPtHokenInf(ptId, hokenPid, sinDate);

                var result = new CoKarte1Model(ptInf, ptByomeis, ptHokenInf);

                return result;
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
