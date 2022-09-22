namespace UseCase.PatientGroupMst.SaveList;

public class SaveListPatientGroupMstInputItem
{
    public SaveListPatientGroupMstInputItem(int groupId, int sortNo, string groupName, List<SaveListPatientGroupDetailMstInputItem> details)
    {
        GroupId = groupId;
        SortNo = sortNo;
        GroupName = groupName;
        Details = details;
    }

    public int GroupId { get; private set; }

    public int SortNo { get; private set; }

    public string GroupName { get; private set; }

    public List<SaveListPatientGroupDetailMstInputItem> Details { get; private set; }
}
