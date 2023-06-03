using Reporting.Mappers.Common;

namespace Reporting.Sokatu.HikariDisk.Service;

public interface IHikariDiskCoReportService
{
    CommonReportingRequestModel GetHikariDiskPrintData(int hpId, int seikyuYm, int hokenKbn, int diskKind, int diskCnt);
}
