using Reporting.Model.ExportKarte1;

namespace Reporting;

public interface IReporting
{
   Karte1ExportModel GetDataKarte1(int hpId, long ptId, int sinDate, int hokenPid, bool tenkiByomei);
}
