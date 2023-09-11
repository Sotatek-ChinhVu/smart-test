namespace Domain.Models.MainMenu;

public class StatisticMenuModel
{
    public StatisticMenuModel(int menuId, int grpId, int reportId, int sortNo, string menuName, int isPrint, List<StaConfModel> staConfigList)
    {
        MenuId = menuId;
        GrpId = grpId;
        ReportId = reportId;
        SortNo = sortNo;
        MenuName = menuName;
        IsPrint = isPrint;
        IsDeleted = false;
        StaConfigList = staConfigList;
    }

    public StatisticMenuModel(int menuId, int grpId, int reportId, int sortNo, string menuName, int isPrint, List<StaConfModel> staConfigList, bool isDeleted, bool isSaveTemp)
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

    public List<StaConfModel> StaConfigList { get; private set; }

    public bool IsDeleted { get; private set; }

    public bool IsSaveTemp { get; private set; }

    public bool IsModified { get; private set; }
}
