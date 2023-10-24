using Reporting.Kensalrai.Model;
using Reporting.Mappers.Common;

namespace Reporting.Kensalrai.Service
{
    public interface IKensaIraiCoReportService
    {
        CommonReportingRequestModel GetKensalraiData(int hpId, int systemDate, int fromDate, int toDate, string centerCd);

        CommonReportingRequestModel GetKensalraiData(int hpId, int systemDate, int fromDate, int toDate, string centerCd, List<KensaIraiModel> kensaIrais);

        List<string> GetIraiFileData(string centerCd, List<KensaIraiModel> kensaIrais, int fileType = 0);

        List<string> GetIraiFileDataDummy(string centerCd, List<KensaIraiModel> kensaIrais, int fileType = 0);
    }
}
