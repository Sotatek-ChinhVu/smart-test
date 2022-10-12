using static Helper.Constants.KarteConst;

namespace Domain.Models.KarteInfs
{
    public class KarteInfModel
    {
        public KarteInfModel(int hpId, long raiinNo, int karteKbn, long seqNo, long ptId, int sinDate, string text, int isDeleted, string richText, DateTime createDate, DateTime updateDate, string createName)
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
            CreateName = createName;
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
        public string CreateName { get; private set; }

        public KarteValidationStatus Validation()
        {
            if (HpId <= 0)
            {
                return KarteValidationStatus.InvalidHpId;
            }
            if (RaiinNo <= 0)
            {
                return KarteValidationStatus.InvalidRaiinNo;
            }
            if (PtId <= 0)
            {
                return KarteValidationStatus.InvalidPtId;
            }
            if (SinDate <= 0)
            {
                return KarteValidationStatus.InvalidSinDate;
            }
            if (IsDeleted < 0)
            {
                return KarteValidationStatus.InvalidIsDelted;
            }

            return KarteValidationStatus.Valid;
        }
    }
}
