namespace Domain.Models.ChartApproval
{
    public class ApprovalInfModel
    {
        public ApprovalInfModel(int hpId, int id, long raiinNo, int seqNo, long ptId, int sinDate, int isDeleted, long ptNum, string kanaName, string name, int kaId, int uketukeNo, string kaName, string tantoName)
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
            KaName = kaName;
            TanToName = tantoName;
        }

        public ApprovalInfModel(int id, int hpId, long ptId, int sinDate, long raiinNo, int isDeleted)
        {
            Id = id;
            HpId = hpId;
            IsDeleted = isDeleted;
            RaiinNo = raiinNo;
            PtId = ptId;
            SinDate = sinDate;
            KanaName = string.Empty;
            Name = string.Empty;
            KaName = string.Empty;
            TanToName = string.Empty;
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

        public string KaName { get; private set; }

        public string TanToName { get; private set; }
    }
}
