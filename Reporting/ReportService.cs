using Infrastructure.Interfaces;
using Reporting.Interface;
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

        public CoNameLabelModel GetNameLabelReportingData(long ptId, string kanjiName, int sinDate)
        {
            using (var noTrackingDataContext = _tenantProvider.GetNoTrackingDataContext())
            {
                var finder = new CoNameLabelFinder(noTrackingDataContext);

                // 患者情報
                CoPtInfModel ptInf = finder.FindPtInf(ptId);

                return new CoNameLabelModel(ptInf, kanjiName, sinDate);
            }
        }
    }
}
