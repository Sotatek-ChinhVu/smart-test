using Reporting.Karte1.Mapper;
using Reporting.Mappers.Common;

namespace Reporting.Karte1.Service;

public interface IKarte1Service
{
    CommonReportingRequestModel GetKarte1ReportingData(int hpId, long ptId, int sinDate, int hokenPid, bool tenkiByomei, bool syuByomei);
}
