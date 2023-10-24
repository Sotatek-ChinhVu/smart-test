using Domain.Models.PatientInfor;

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
        PatientManagement = new();
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
        PatientManagement = new();
    }

    public StatisticMenuModel(int menuId, int grpId, int reportId, string menuName, int sortNo, bool isDeleted, bool isModified, PatientManagementModel patientManagement)
    {
        MenuId = menuId;
        GrpId = grpId;
        ReportId = reportId;
        MenuName = menuName;
        SortNo = sortNo;
        IsDeleted = isDeleted;
        IsModified = isModified;
        StaConfigList = new();
        PatientManagement = patientManagement;
    }

    public StatisticMenuModel()
    {
        MenuName = string.Empty;
        StaConfigList = new();
        PatientManagement = new();
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

    public PatientManagementModel PatientManagement { get; private set; }
}
