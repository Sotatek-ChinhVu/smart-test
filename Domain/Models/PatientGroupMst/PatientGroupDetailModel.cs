namespace Domain.Models.PatientGroupMst
{
    public class PatientGroupDetailModel
    {
        public int GroupID { get; private set; }

        public string GroupCode { get; private set; }

        public long SeqNo { get; private set; }

        public int SortNo { get; private set; }

        public string GroupDetailName { get; private set; }

        public PatientGroupDetailModel(int groupID, string groupCode, long seqNo, int sortNo, string groupDetailName)
        {
            GroupID = groupID;
            GroupCode = groupCode;
            SeqNo = seqNo;
            SortNo = sortNo;
            GroupDetailName = groupDetailName;
        }
    }
}
