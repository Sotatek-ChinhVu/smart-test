namespace Domain.Models.PatientGroupMst
{
    public class PatientGroupMstModel
    {
        public int GroupId { get; private set; }

        public int SortNo { get; private set; }

        public string GroupName { get; private set; }

        public List<PatientGroupDetailModel> Details { get; private set; }

        public PatientGroupMstModel(int groupId, int sortNo, string groupName, List<PatientGroupDetailModel> details)
        {
            GroupId = groupId;
            SortNo = sortNo;
            GroupName = groupName;
            Details = details;
        }
    }
}
