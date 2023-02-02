using Helper.Common;
using Infrastructure.Interfaces;
using Reporting.Interface;
using Reporting.Karte1.DB;
using Reporting.Karte1.Model;
using Reporting.Model.ExportKarte1;
using Reporting.NameLabel.DB;
using Reporting.NameLabel.Models;
using Reporting.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Reporting
{
    public class ReportService : IReportService
    {
        private readonly ITenantProvider _tenantProvider;
        private readonly IExportKarte1 _exportKarte1;

        public ReportService(ITenantProvider tenantProvider, IExportKarte1 exportKarte1)
        {
            _tenantProvider = tenantProvider;
            _exportKarte1 = exportKarte1;
        }

        public Karte1ExportModel GetDataKarte1(int hpId, long ptId, int sinDate, int hokenPid, bool tenkiByomei, bool syuByomei)
        {
            return _exportKarte1.GetDataKarte1(hpId, ptId, sinDate, hokenPid, tenkiByomei, syuByomei);
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
