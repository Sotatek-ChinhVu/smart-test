namespace Domain.Models.OrdInfs
{
    public class ApproveInfModel
    {
        public ApproveInfModel(int id, int hpId, long ptId, int sinDate, long raiinNo, int seqNo, int isDeleted, string displayApprovalInfo)
        {
            Id = id;
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            SeqNo = seqNo;
            IsDeleted = isDeleted;
            DisplayApprovalInfo = displayApprovalInfo;
        }

        public int Id { get; private set; }
        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public int SinDate { get; private set; }
        public long RaiinNo { get; private set; }
        public int SeqNo { get; private set; }
        public int IsDeleted { get; private set; }
        public string DisplayApprovalInfo { get; private set; }
    }
}
