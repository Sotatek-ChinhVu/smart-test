using Domain.Models.MainMenu;

namespace UseCase.MainMenu;

public class StatisticMenuItem
{
    public StatisticMenuItem(StatisticMenuModel model)
    {
        MenuId = model.MenuId;
        GrpId = model.GrpId;
        ReportId = model.ReportId;
        SortNo = model.SortNo;
        MenuName = model.MenuName;
        IsPrint = model.IsPrint;
        StaConfigList = model.StaConfigList.Select(item => new StaConfItem(item)).ToList();
        IsDeleted = model.IsDeleted;
    }

    public StatisticMenuItem(int menuId, int grpId, int reportId, int sortNo, string menuName, int isPrint, List<StaConfItem> staConfigList, bool isDeleted, bool isSaveTemp)
    {
        MenuId = menuId;
        GrpId = grpId;
        ReportId = reportId;
        SortNo = sortNo;
        MenuName = menuName;
        IsPrint = isPrint;
        StaConfigList = staConfigList;
        IsDeleted = isDeleted;
        IsSaveTemp = isSaveTemp;
    }

    public int MenuId { get; private set; }

    public int GrpId { get; private set; }

    public int ReportId { get; private set; }

    public int SortNo { get; private set; }

    public string MenuName { get; private set; }

    public int IsPrint { get; private set; }

    public List<StaConfItem> StaConfigList { get; private set; }

    public bool IsDeleted { get; private set; }

    public bool IsSaveTemp { get; private set; }
}
