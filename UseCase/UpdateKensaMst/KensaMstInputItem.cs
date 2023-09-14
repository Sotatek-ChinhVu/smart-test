﻿namespace UseCase.UpdateKensaMst
{
    public class KensaMstInputItem
    {
        public KensaMstInputItem(string kensaItemCd, int kensaItemSeqNo, string centerCd, string kensaName, string kensaKana, string unit, int materialCd, int containerCd, string maleStd, string maleStdLow, string maleStdHigh, string femaleStd, string femaleStdLow, string femaleStdHigh, string formula, int digit, string oyaItemCd, int oyaItemSeqNo, long sortNo, string centerItemCd1, string centerItemCd2, int isDeleted)
        {
            KensaItemCd = kensaItemCd;
            KensaItemSeqNo = kensaItemSeqNo;
            CenterCd = centerCd;
            KensaName = kensaName;
            KensaKana = kensaKana;
            Unit = unit;
            MaterialCd = materialCd;
            ContainerCd = containerCd;
            MaleStd = maleStd;
            MaleStdLow = maleStdLow;
            MaleStdHigh = maleStdHigh;
            FemaleStd = femaleStd;
            FemaleStdLow = femaleStdLow;
            FemaleStdHigh = femaleStdHigh;
            Formula = formula;
            Digit = digit;
            OyaItemCd = oyaItemCd;
            OyaItemSeqNo = oyaItemSeqNo;
            SortNo = sortNo;
            CenterItemCd1 = centerItemCd1;
            CenterItemCd2 = centerItemCd2;
            IsDeleted = isDeleted;
        }

        public string KensaItemCd { get; private set; }

        public int KensaItemSeqNo { get; private set; }

        public string CenterCd { get; private set; } 

        public string KensaName { get; private set; } 

        public string KensaKana { get; private set; } 

        public string Unit { get; private set; } 

        public int MaterialCd { get; private set; }

        public int ContainerCd { get; private set; }

        public string MaleStd { get; private set; } 

        public string MaleStdLow { get; private set; } 

        public string MaleStdHigh { get; private set; } 

        public string FemaleStd { get; private set; } 

        public string FemaleStdLow { get; private set; } 

        public string FemaleStdHigh { get; private set; } 

        public string Formula { get; private set; } 

        public int Digit { get; private set; }

        public string OyaItemCd { get; private set; } 

        public int OyaItemSeqNo { get; private set; }

        public long SortNo { get; private set; }

        public string CenterItemCd1 { get; private set; } 

        public string CenterItemCd2 { get; private set; }

        public int IsDeleted { get; private set; }
    }
}
