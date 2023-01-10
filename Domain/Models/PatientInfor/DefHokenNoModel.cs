using static Helper.Constants.DefHokenNoConst;

namespace Domain.Models.PatientInfor
{
    public class DefHokenNoModel
    {
        public DefHokenNoModel(string digit1, string digit2, string digit3, string digit4, string digit5, string digit6, string digit7, string digit8, long seqNo, int hokenNo, int hokenEdaNo, int sortNo, int isDeleted)
        {
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
            if ((!int.TryParse(Digit1, out int d1) && (d1 < 0 || d1 > 9)) || string.IsNullOrEmpty(Digit1))
                return ValidationStatus.InvalidDigit1;

            if ((!int.TryParse(Digit2, out int d2) && (d2 < 0 || d2 > 9)) || string.IsNullOrEmpty(Digit2))
                return ValidationStatus.InvalidDigit2;

            if ((!int.TryParse(Digit3, out int d3) && (d3 < 0 || d3 > 9)) && !string.IsNullOrEmpty(Digit3))
                return ValidationStatus.InvalidDigit3;

            if ((!int.TryParse(Digit4, out int d4) && (d4 < 0 || d4 > 9)) && !string.IsNullOrEmpty(Digit4))
                return ValidationStatus.InvalidDigit4;

            if ((!int.TryParse(Digit5, out int d5) && (d5 < 0 || d5 > 9)) && !string.IsNullOrEmpty(Digit5))
                return ValidationStatus.InvalidDigit5;

            if ((!int.TryParse(Digit6, out int d6) && (d6 < 0 || d6 > 9)) && !string.IsNullOrEmpty(Digit6))
                return ValidationStatus.InvalidDigit6;

            if ((!int.TryParse(Digit7, out int d7) && (d7 < 0 || d7 > 9)) && !string.IsNullOrEmpty(Digit7))
                return ValidationStatus.InvalidDigit7;

            if ((!int.TryParse(Digit8, out int d8) && (d8 < 0 || d8 > 9)) && !string.IsNullOrEmpty(Digit8))
                return ValidationStatus.InvalidDigit8;

            if (HokenNo < 0)
                return ValidationStatus.InvalidHokenNo;

            #endregion
            return ValidationStatus.Valid;
        }
    }
}
