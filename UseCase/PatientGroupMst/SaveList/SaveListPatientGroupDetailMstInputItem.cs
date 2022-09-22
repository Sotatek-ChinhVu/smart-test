namespace UseCase.PatientGroupMst.SaveList;

public class SaveListPatientGroupDetailMstInputItem
{
    public SaveListPatientGroupDetailMstInputItem(int groupId, string groupCode, long seqNo, string groupDetailName)
    {
        GroupId = groupId;
        GroupCode = groupCode;
        SeqNo = seqNo;
        GroupDetailName = groupDetailName;
    }

    public int GroupId { get; private set; }

    public string GroupCode { get; private set; }

    public long SeqNo { get; private set; }

    public string GroupDetailName { get; private set; }
}
