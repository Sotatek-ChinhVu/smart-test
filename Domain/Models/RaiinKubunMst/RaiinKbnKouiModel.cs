namespace Domain.Models.RaiinKubunMst
{
    public class RaiinKbnKouiModel
    {
        public RaiinKbnKouiModel(int hpId,int grpId, int kbnCd, int seqNo, int kouiKbnId, int isDeleted)
        {
            HpId = hpId;
            GrpId = grpId;
            KbnCd = kbnCd;
            SeqNo = seqNo;
            KouiKbnId = kouiKbnId;
            IsDeleted = isDeleted;
        }
        public int HpId { get; private set; }
        public int GrpId { get; private set; }

        public int KbnCd { get; private set; }

        public int SeqNo { get; private set; }

        public int KouiKbnId { get; private set; }

        public int IsDeleted { get; private set; }

    }
}
