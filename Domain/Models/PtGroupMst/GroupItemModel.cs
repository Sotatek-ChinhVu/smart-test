namespace Domain.Models.PtGroupMst
{
    public class GroupItemModel
    {
        public GroupItemModel(int grpId, string grpCode, long seqNo, string grpCodeName, int sortNo, int isDeleted)
        {
            GrpId = grpId;
            GrpCode = grpCode;
            SeqNo = seqNo;
            GrpCodeName = grpCodeName;
            SortNo = sortNo;
            IsDeleted = isDeleted;
        }

        public int GrpId { get; private set; }

        public string GrpCode { get; private set; }

        public long SeqNo { get; private set; }

        public string GrpCodeName { get; private set; }

        public int SortNo { get; private set; }

        public int IsDeleted { get; private set; }
    }
}
