using Reporting.Model.ExportKarte1;

namespace Reporting.Interface;

public interface IExportKarte1
{
    Karte1ExportModel GetDataKarte1(int hpId, long ptId, int sinDate, int hokenPid, bool tenkiByomei, bool syuByomei);
}
