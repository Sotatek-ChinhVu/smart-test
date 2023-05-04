namespace Domain.Models.MainMenu;

public class StaGrpModel
{
    public StaGrpModel(int grpId, int reportId, string reportName, int sortNo)
    {
        GrpId = grpId;
        ReportId = reportId;
        ReportName = reportName;
        SortNo = sortNo;
    }

    public int GrpId { get; private set; }

    public int ReportId { get; private set; }

    public string ReportName { get; private set; }

    public int SortNo { get; private set; }
}
