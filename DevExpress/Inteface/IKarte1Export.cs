namespace DevExpress.Inteface;

public interface IKarte1Export
{
    MemoryStream ExportToPdf(int hpId, long ptId, int sinDate, int hokenPid, bool tenkiByomei);
}
