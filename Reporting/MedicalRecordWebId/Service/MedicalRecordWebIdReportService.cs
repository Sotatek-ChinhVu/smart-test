using Infrastructure.Interfaces;
using Reporting.CommonMasters.Config;
using Reporting.Mappers.Common;
using Reporting.MedicalRecordWebId.DB;
using Reporting.MedicalRecordWebId.Mapper;
using Reporting.MedicalRecordWebId.Model;

namespace Reporting.MedicalRecordWebId.Service;

public class MedicalRecordWebIdReportService : IMedicalRecordWebIdReportService
{
    private readonly ITenantProvider _tenantProvider;
    private readonly ISystemConfig _systemConfig;

    public MedicalRecordWebIdReportService(ITenantProvider tenantProvider, ISystemConfig systemConfig)
    {
        _tenantProvider = tenantProvider;
        _systemConfig = systemConfig;
    }

    public CommonReportingRequestModel GetMedicalRecordWebIdReportingData(int hpId, long ptId, int sinDate)
    {
        using (var noTrackingDataContext = _tenantProvider.GetNoTrackingDataContext())
        {
            var finder = new CoMedicalRecordWebIdFinder(_tenantProvider);

            try
            {
                // データ取得
                var coModel = GetData(finder, hpId, sinDate, ptId);

                return new CoMedicalRecordWebIdMapper(coModel).GetData();
            }
            finally
            {
                finder.ReleaseResource();
                _tenantProvider.DisposeDataContext();
                _systemConfig.ReleaseResource();
            }
        }
    }

    /// <summary>
    /// 印刷するデータを取得する
    /// </summary>
    /// <returns></returns>
    private CoMedicalRecordWebIdModel GetData(CoMedicalRecordWebIdFinder finder, int hpId, int sinDate, long ptId)
    {
        var hpInf = finder.GetHpInf(hpId, sinDate);
        var ptInf = finder.GetPtInf(hpId, ptId);
        var ptJibkar = finder.GetPtJibkar(hpId, ptId);
        var webIdQrCode = _systemConfig.WebIdQrCode(hpId);
        var medicalInstitutionCode = _systemConfig.MedicalInstitutionCode(hpId);
        var webIdUrlForPc = _systemConfig.WebIdUrlForPc(hpId);

        return new CoMedicalRecordWebIdModel(sinDate, hpInf, ptInf, ptJibkar, webIdQrCode, medicalInstitutionCode, webIdUrlForPc);
    }
}
