using Domain.Models.MainMenu;

namespace UseCase.MainMenu;

public class StaGrpItem
{
    public StaGrpItem(StaGrpModel model)
    {
        GrpId = model.GrpId;
        ReportId = model.ReportId;
        ReportName = model.ReportName;
        SortNo = model.SortNo;
    }

    public int GrpId { get; private set; }

    public int ReportId { get; private set; }

    public string ReportName { get; private set; }

    public int SortNo { get; private set; }
}
