using static Helper.Constants.DefHokenNoConst;

namespace Domain.Models.PatientInfor
{
    public class DefHokenNoModel
    {
        public DefHokenNoModel(int hpId, string digit1, string digit2, string digit3, string digit4, string digit5, string digit6, string digit7, string digit8, long seqNo, int hokenNo, int hokenEdaNo, int sortNo, int isDeleted)
        {
            HpId = hpId;
            Digit1 = digit1;
            Digit2 = digit2;
            Digit3 = digit3;
            Digit4 = digit4;
            Digit5 = digit5;
            Digit6 = digit6;
            Digit7 = digit7;
            Digit8 = digit8;
            SeqNo = seqNo;
            HokenNo = hokenNo;
            HokenEdaNo = hokenEdaNo;
            SortNo = sortNo;
            IsDeleted = isDeleted;
        }

        public int HpId { get; private set; }
        public string Digit1 { get; private set; }
        public string Digit2 { get; private set; }
        public string Digit3 { get; private set; }
        public string Digit4 { get; private set; }
        public string Digit5 { get; private set; }
        public string Digit6 { get; private set; }
        public string Digit7 { get; private set; }
        public string Digit8 { get; private set; }
        public long SeqNo { get; private set; }
        public int HokenNo { get; private set; }
        public int HokenEdaNo { get; private set; }
        public int SortNo { get; private set; }
        public int IsDeleted { get; private set; }

        public ValidationStatus Validation()
        {
            #region common
            if (HpId <= 0)
                return ValidationStatus.InvalidHpId;
            if (Int32.Parse(Digit1) < 0 || Int32.Parse(Digit1) > 9)
                return ValidationStatus.InvalidDigit1;

            if (Int32.Parse(Digit2) < 0 || Int32.Parse(Digit2) > 9)
                return ValidationStatus.InvalidDigit2;

            if (Int32.Parse(Digit3) < 0 || Int32.Parse(Digit3) > 9)
                return ValidationStatus.InvalidDigit3;

            if (Int32.Parse(Digit4) < 0 || Int32.Parse(Digit4) > 9)
                return ValidationStatus.InvalidDigit4;

            if (Int32.Parse(Digit5) < 0 || Int32.Parse(Digit5) > 9)
                return ValidationStatus.InvalidDigit5;

            if (Int32.Parse(Digit6) < 0 || Int32.Parse(Digit6) > 9)
                return ValidationStatus.InvalidDigit6;

            if (Int32.Parse(Digit7) < 0 || Int32.Parse(Digit7) > 9)
                return ValidationStatus.InvalidDigit7;

            if (Int32.Parse(Digit8) < 0 || Int32.Parse(Digit8) > 9)
                return ValidationStatus.InvalidDigit8;
            #endregion
            return ValidationStatus.Valid;
        }
    }
}
