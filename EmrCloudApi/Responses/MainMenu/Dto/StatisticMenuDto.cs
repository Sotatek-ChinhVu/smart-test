using UseCase.MainMenu;

namespace EmrCloudApi.Responses.MainMenu.Dto;

public class StatisticMenuDto
{
    public StatisticMenuDto(StatisticMenuItem model)
    {
        MenuId = model.MenuId;
        GrpId = model.GrpId;
        ReportId = model.ReportId;
        SortNo = model.SortNo;
        MenuName = model.MenuName;
        IsPrint = model.IsPrint;
        StaConfigList = model.StaConfigList.Select(item => new StaConfDto(item)).ToList();
    }

    public int MenuId { get; private set; }

    public int GrpId { get; private set; }

    public int ReportId { get; private set; }

    public int SortNo { get; private set; }

    public string MenuName { get; private set; }

    public int IsPrint { get; private set; }

    public List<StaConfDto> StaConfigList { get; private set; }
}
