namespace Domain.Models.RaiinKubunMst
{
    public class RaiinKbnYayokuModel
    {
        public RaiinKbnYayokuModel(int hpId, int kbnCd, long seqNo, int yoyakuCd, int isDeleted)
        {
            HpId = hpId;
            KbnCd = kbnCd;
            SeqNo = seqNo;
            YoyakuCd = yoyakuCd;
            IsDeleted = isDeleted;
        }

        public int HpId { get; private set; }

        public int GrpId { get; private set; }

        public int KbnCd { get; private set; }

        public long SeqNo { get; private set; }

        public int YoyakuCd { get; private set; }

        public int IsDeleted { get; private set; }

        public RaiinKbnYayokuModel ChangeYoyakuCd(int yoyakuCd)
        {
            YoyakuCd = yoyakuCd;
            return this;
        }
    }
}
