using UseCase.MainMenu;

namespace EmrCloudApi.Responses.MainMenu.Dto;

public class StaGrpDto
{
    public StaGrpDto(StaGrpItem model)
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
