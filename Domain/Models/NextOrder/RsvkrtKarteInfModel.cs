using static Helper.Constants.KarteConst;

namespace Domain.Models.NextOrder
{
    public class RsvkrtKarteInfModel
    {
        public RsvkrtKarteInfModel(int hpId, long ptId, int rsvDate, long rsvkrtNo, long seqNo, string text, string richText, int isDeleted)
        {
            HpId = hpId;
            PtId = ptId;
            RsvDate = rsvDate;
            RsvkrtNo = rsvkrtNo;
            SeqNo = seqNo;
            Text = text;
            RichText = richText;
            IsDeleted = isDeleted;
        }

        public RsvkrtKarteInfModel()
        {
            HpId = 0;
            PtId = 0;
            RsvDate = 0;
            RsvkrtNo = 0;
            SeqNo = 0;
            Text = string.Empty;
            RichText = string.Empty;
            IsDeleted = 0;
        }

        public KarteValidationStatus Validation()
        {
            if (HpId <= 0)
            {
                return KarteValidationStatus.InvalidHpId;
            }
            if (RsvkrtNo < 0)
            {
                return KarteValidationStatus.InvalidRaiinNo;
            }
            if (PtId <= 0)
            {
                return KarteValidationStatus.InvalidPtId;
            }
            if (RsvDate <= 0)
            {
                return KarteValidationStatus.InvalidSinDate;
            }
            if (IsDeleted < 0)
            {
                return KarteValidationStatus.InvalidIsDelted;
            }

            return KarteValidationStatus.Valid;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int RsvDate { get; private set; }

        public long RsvkrtNo { get; private set; }

        public long SeqNo { get; private set; }

        public string Text { get; private set; }

        public string RichText { get; private set; }

        public int IsDeleted { get; private set; }
    }
}
