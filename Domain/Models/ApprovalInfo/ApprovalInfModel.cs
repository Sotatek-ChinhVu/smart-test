namespace Domain.Models.ApprovalInfo
{
    public class ApprovalInfModel
    {
        public ApprovalInfModel(int hpId, int id, long raiinNo, int seqNo, long ptId, int sinDate, int isDeleted, long ptNum, string kanaName, string name, int kaId, int uketukeNo)
        {
            HpId = hpId;
            Id = id;
            RaiinNo = raiinNo;
            SeqNo = seqNo;
            PtId = ptId;
            SinDate = sinDate;
            IsDeleted = isDeleted;
            PtNum = ptNum;
            KanaName = kanaName;
            Name = name;
            UketokeNo = uketukeNo;
            KaId = kaId;
        }
        public ApprovalInfModel(int id, int hpId, long ptId, int sinDate, long raiiNo, int seqNo, int isDeleted, DateTime createDate, int createId, string createMachine, DateTime updateDate, int updateId, string updateMachine)
        {
            Id = id;
            HpId = hpId;
            IsDeleted = isDeleted;
            RaiiNo = raiiNo;
            SeqNo = seqNo;
            PtId = ptId;
            SinDate = sinDate;
            CreateId = createId;
            CreateDate = createDate;
            CreateMachine = createMachine;
            UpdateId = updateId;
            UpdateDate = updateDate;
            UpdateMachine = updateMachine;
        }
        public int HpId { get; private set; }
        public int Id { get; private set; }
        public long RaiinNo { get; private set; }
        public int SeqNo { get; private set; }
        public long PtId { get; private set; }
        public int SinDate { get; private set; }
        public int IsDeleted { get; private set; }
        public long PtNum { get; private set; }
        public string KanaName { get; private set; }
        public string Name { get; private set; }
        public int UketokeNo { get; private set; }
        public int KaId { get; private set; }
        public long RaiiNo { get; private set; }
        public DateTime CreateDate { get; private set; }
        public string CreateMachine { get; private set; }
        public int CreateId { get; private set; }
        public DateTime UpdateDate { get; private set; }
        public string UpdateMachine { get; private set; }
        public int UpdateId { get; private set; }
    }
}
