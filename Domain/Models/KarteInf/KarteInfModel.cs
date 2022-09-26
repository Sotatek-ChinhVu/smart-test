using static Helper.Constants.TodayKarteConst;

namespace Domain.Models.KarteInfs
{
    public class KarteInfModel
    {
        public KarteInfModel(int hpId, long raiinNo, int karteKbn, long seqNo, long ptId, int sinDate, string text, int isDeleted, string richText, DateTime createDate, DateTime updateDate)
        {
            HpId = hpId;
            RaiinNo = raiinNo;
            KarteKbn = karteKbn;
            SeqNo = seqNo;
            PtId = ptId;
            SinDate = sinDate;
            Text = text;
            IsDeleted = isDeleted;
            RichText = richText;
            CreateDate = createDate;
            UpdateDate = updateDate;
        }

        public int HpId { get; private set; }
        public long RaiinNo { get; private set; }
        public int KarteKbn { get; private set; }
        public long SeqNo { get; private set; }
        public long PtId { get; private set; }
        public int SinDate { get; private set; }
        public string Text { get; private set; }
        public int IsDeleted { get; private set; }
        public string RichText { get; private set; }
        public DateTime CreateDate { get; private set; }
        public DateTime UpdateDate { get; private set; }

        public TodayKarteValidationStatus Validation()
        {
            if (HpId <= 0)
            {
                return TodayKarteValidationStatus.InvalidHpId;
            }
            if (RaiinNo <= 0)
            {
                return TodayKarteValidationStatus.InvalidRaiinNo;
            }
            if (PtId <= 0)
            {
                return TodayKarteValidationStatus.InvalidPtId;
            }
            if (SinDate <= 0)
            {
                return TodayKarteValidationStatus.InvalidSinDate;
            }
            if (IsDeleted < 0)
            {
                return TodayKarteValidationStatus.InvalidIsDelted;
            }

            return TodayKarteValidationStatus.Valid;
        }
    }
}
