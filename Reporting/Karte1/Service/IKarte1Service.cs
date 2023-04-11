using Reporting.Karte1.Mapper;

namespace Reporting.Karte1.Service;

public interface IKarte1Service
{
    Karte1Mapper GetKarte1ReportingData(int hpId, long ptId, int sinDate, int hokenPid, bool tenkiByomei, bool syuByomei);
}
