using Entity.Tenant;

namespace Reporting.PatientManagement.Models;

public class GroupInfoSearchPatientModel
{
    public PtGrpNameMst PtGrpNameMst { get; private set; }

    public GroupInfoSearchPatientModel(PtGrpNameMst ptGrpNameMst, GroupItemModel ptGrpItem = null)
    {
        PtGrpNameMst = ptGrpNameMst;
        GroupItemSelected = ptGrpItem;
    }

    public long PtId
    {
        get; set;
    }
    public int GroupId
    {
        get
        {
            if (PtGrpNameMst is null)
            {
                return 0;
            }

            return PtGrpNameMst.GrpId;
        }
    }

    public string GroupName
    {
        get
        {
            if (PtGrpNameMst is null)
            {
                return string.Empty;
            }

            return PtGrpNameMst.GrpName ?? string.Empty;
        }
    }

    public int GroupSortNo
    {
        get
        {
            if (PtGrpNameMst is null)
            {
                return 0;
            }

            return PtGrpNameMst.SortNo;
        }
    }

    public GroupItemModel GroupItemSelected { get; set; }

    public string GroupCodeSelected
    {
        get
        {
            if (GroupItemSelected is null)
            {
                return string.Empty;
            }

            return GroupItemSelected.GroupCode;
        }
    }

    public List<GroupItemModel> ListItem { get; set; }

    public bool CheckDefaultValue()
    {
        return false;
    }
}

public class GroupItemModel
{
    private readonly PtGrpItem _groupItem;
    public PtGrpItem GroupItem
    {
        get
        {
            return _groupItem;
        }
    }

    public GroupItemModel(PtGrpItem groupItem)
    {
        _groupItem = groupItem;
    }

    public int GroupId
    {
        get
        {
            if (_groupItem is null)
            {
                return 0;
            }

            return _groupItem.GrpId;
        }
    }

    public string GroupCode
    {
        get
        {
            if (_groupItem is null)
            {
                return string.Empty;
            }

            return _groupItem.GrpCode;
        }
    }

    public string GroupCodeName
    {
        get
        {
            if (_groupItem is null)
            {
                return string.Empty;
            }

            return _groupItem.GrpCodeName ?? string.Empty;
        }
    }

    public string GroupItemName
    {
        get
        {
            if (_groupItem is null)
            {
                return string.Empty;
            }

            return _groupItem.GrpCode + " " + _groupItem.GrpCodeName;
        }
    }

    public int GroupSortNo
    {
        get
        {
            if (_groupItem is null)
            {
                return 0;
            }

            return _groupItem.SortNo;
        }
    }
}
