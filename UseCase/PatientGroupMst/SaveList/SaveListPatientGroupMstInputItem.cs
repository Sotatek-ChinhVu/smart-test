namespace UseCase.PatientGroupMst.SaveList;

public class SaveListPatientGroupMstInputItem
{
    public SaveListPatientGroupMstInputItem(int groupId, string groupName, List<SaveListPatientGroupDetailMstInputItem> details)
    {
        GroupId = groupId;
        GroupName = groupName;
        Details = details;
    }

    public int GroupId { get; private set; }

    public string GroupName { get; private set; }

    public List<SaveListPatientGroupDetailMstInputItem> Details { get; private set; }
}
