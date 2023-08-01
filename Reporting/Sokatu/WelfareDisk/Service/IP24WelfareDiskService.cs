using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareDisk.Service;

public interface IP24WelfareDiskService
{
    CommonExcelReportingModel GetDataP24WelfareDisk(int hpId, int seikyuYm, SeikyuType seikyuType);
}