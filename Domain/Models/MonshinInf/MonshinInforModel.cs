
using static Helper.Constants.MonshinInfConst;

namespace Domain.Models.MonshinInf
{
    public class MonshinInforModel
    {
        public MonshinInforModel(int hpId, long ptId, long raiinNo, int sinDate, string text, string rtext, int getKbn, int isDeleted)
        {
            HpId = hpId;
            PtId = ptId;
            RaiinNo = raiinNo;
            SinDate = sinDate;
            Text = text;
            Rtext = rtext;
            GetKbn = getKbn;
            IsDeleted = isDeleted;
        }

        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public long RaiinNo { get; private set; }
        public int SinDate { get; private set; }
        public string Text { get; private set; }
        public string Rtext { get; private set; }
        public int GetKbn { get; private set; }
        public int IsDeleted { get; private set; }

        public ValidationStatus Validation()
        {
            #region common
            if (HpId <= 0)
                return ValidationStatus.InvalidHpId;

            if (PtId <= 0)
                return ValidationStatus.InvalidPtId;

            if (RaiinNo <= 0)
                return ValidationStatus.InValidRaiinNo;

            if (SinDate <= 0)
                return ValidationStatus.InvalidSinDate;
            #endregion
            return ValidationStatus.Valid;
        }
    }
}
