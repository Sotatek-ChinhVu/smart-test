using Reporting.Mappers.Common;
using Reporting.PatientManagement.DB;
using Reporting.PatientManagement.Models;
using Reporting.Statistics.Sta9000.Service;
using static Reporting.Statistics.Sta9000.Models.CoSta9000PtConf;

namespace Reporting.PatientManagement.Service;

public class PatientManagementService : IPatientManagementService
{

    private readonly IPatientManagementFinder _finder;
    private readonly ISta9000CoReportService _sta9000CoReportService;

    public PatientManagementService(IPatientManagementFinder finder, ISta9000CoReportService sta9000CoReportService)
    {
        _finder = finder;
        _sta9000CoReportService = sta9000CoReportService;
    }

    public CommonReportingRequestModel PrintData(int hpId, PatientManagementModel patientManagementModel)
    {
        try
        {
            var groupInfos = _finder.GetListGroupInfo(hpId);
            var coSta9000PtConf = patientManagementModel.IsDefaultPtConfInView() ? null : patientManagementModel?.AsCoSta9000PtConf(groupInfos.Where(x => x.GroupItemSelected != null && !string.IsNullOrEmpty(x.GroupCodeSelected)).Select(x => new PtGrp(x.GroupId, x.GroupCodeSelected)).ToList());
            var coSta9000HokenConf = patientManagementModel!.IsDefaultHokenConfInView() ? null : patientManagementModel?.AsCoSta9000HokenConf();
            var coSta9000ByomeiConf = patientManagementModel!.IsDefaultByomeiConfInView() ? null : patientManagementModel?.AsCoSta9000ByomeiConf();
            var coSta9000KarteConf = patientManagementModel!.IsDefaultKarteConfInView() ? null : patientManagementModel?.AsCoSta9000KarteConf();
            var coSta9000SinConf = patientManagementModel!.IsDefaultSinConfInView() ? null : patientManagementModel?.AsCoSta9000SinConf();
            var coSta9000KensaConf = patientManagementModel!.IsDefaultKensaConfInView() ? null : patientManagementModel?.AsCoSta9000KensaConf();
            var coSta9000RaiinConf = patientManagementModel!.IsDefaultRaiinConfInView() ? null : patientManagementModel?.AsCoSta9000RaiinConf();
            return _sta9000CoReportService.GetSta9000ReportingData(hpId, patientManagementModel!.ReportType, patientManagementModel.OutputOrder, patientManagementModel.OutputOrder2, patientManagementModel.OutputOrder3,
                                                                   coSta9000PtConf, coSta9000HokenConf, coSta9000ByomeiConf, coSta9000RaiinConf, coSta9000SinConf, coSta9000KarteConf, coSta9000KensaConf);
        }
        finally
        {
            _finder.ReleaseResource();
        }
    }
}
