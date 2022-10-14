﻿namespace Domain.Models.PatientInfor
{
    public class DefHokenNoModel
    {
        public DefHokenNoModel(int hpId, string digit1, string digit2, string digit3, string digit4, string digit5, string digit6, string digit7, string digit8, int hokenNo, int hokenEdaNo, int sortNo, int isDeleted)
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
        public int HokenNo { get; private set; }
        public int HokenEdaNo { get; private set; }
        public int SortNo { get; private set; }
        public int IsDeleted { get; private set; }
    }
}
