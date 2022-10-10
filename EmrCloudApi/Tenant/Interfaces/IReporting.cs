namespace EmrCloudApi.Tenant.Interfaces;

public interface IReporting
{
    Stream ExportKarte1ToPdf(int hpId, long ptId, int sinDate, int hokenPid, bool tenkiByomei);
}
