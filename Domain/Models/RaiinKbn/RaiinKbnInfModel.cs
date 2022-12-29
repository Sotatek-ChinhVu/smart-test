namespace Domain.Models.RaiinKbn
{
    public class RaiinKbnInfModel
    {
        public RaiinKbnInfModel(int hpId, long ptId, int sinDate, long raiinNo, int grpId, long seqNo, int kbnCd, int isDelete)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            GrpId = grpId;
            SeqNo = seqNo;
            KbnCd = kbnCd;
            IsDelete = isDelete;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int SinDate { get; private set; }

        public long RaiinNo { get; private set; }

        public int GrpId { get; private set; }

        public long SeqNo { get; private set; }

        public int KbnCd { get; private set; }

        public int IsDelete { get; private set; }

        public RaiinKbnInfModel ChangeKbnCd(int kbnCd)
        {
            KbnCd = kbnCd;
            return this;
        }
    }
}
