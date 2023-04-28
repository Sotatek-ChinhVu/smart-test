namespace Domain.Models.MainMenu;

public class StatisticMenuModel
{
    public StatisticMenuModel(int menuId, int grpId, int reportId, int sortNo, string menuName, int isPrint)
    {
        MenuId = menuId;
        GrpId = grpId;
        ReportId = reportId;
        SortNo = sortNo;
        MenuName = menuName;
        IsPrint = isPrint;
        IsDeleted = false;
    }

    public StatisticMenuModel(int menuId, int grpId, int reportId, int sortNo, string menuName, int isPrint, bool isDeleted)
    {
        MenuId = menuId;
        GrpId = grpId;
        ReportId = reportId;
        SortNo = sortNo;
        MenuName = menuName;
        IsPrint = isPrint;
        IsDeleted = isDeleted;
    }

    public int MenuId { get; private set; }

    public int GrpId { get; private set; }

    public int ReportId { get; private set; }

    public int SortNo { get; private set; }

    public string MenuName { get; private set; }

    public int IsPrint { get; private set; }

    public bool IsDeleted { get; private set; }
}
