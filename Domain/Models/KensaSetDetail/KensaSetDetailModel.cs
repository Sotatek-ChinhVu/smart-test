namespace Domain.Models.KensaSetDetail
{
    public class KensaSetDetailModel
    {
        public KensaSetDetailModel(int hpId, int setId, int setEdaNo, int setEdaParentNo, string kensaItemCd, string oyaItemCd, string kensaName, int kensaItemSeqNo, int sortNo, List<KensaSetDetailModel> childrens, int isDeleted, string maleStd, string femaleStd, string maleStdLow, string femaleStdLow,
            string maleStdHigh, string femaleStdHigh, string unit, string uniqId, string uniqIdParent)
        {
            HpId = hpId;
            SetId = setId;
            SetEdaNo = setEdaNo;
            SetEdaParentNo = setEdaParentNo;
            KensaItemCd = kensaItemCd;
            OyaItemCd = oyaItemCd;
            KensaName = kensaName;
            KensaItemSeqNo = kensaItemSeqNo;
            SortNo = sortNo;
            Childrens = childrens;
            IsDeleted = isDeleted;
            MaleStd = maleStd;
            FemaleStd = femaleStd;
            MaleStdLow = maleStdLow;
            FemaleStdLow = femaleStdLow;
            MaleStdHigh = maleStdHigh;
            FemaleStdHigh = femaleStdHigh;
            Unit = unit;
            UniqId = uniqId;
            UniqIdParent = uniqIdParent;
        }

        public int HpId { get; private set; }

        public int SetId { get; private set; }

        public int SetEdaNo { get; private set; }
        public int SetEdaParentNo { get; private set; }

        public string KensaItemCd { get; private set; }

        public string OyaItemCd { get; private set; }

        public string KensaName { get; private set; }

        public int KensaItemSeqNo { get; private set; }

        public int SortNo { get; private set; }

        public List<KensaSetDetailModel> Childrens { get; private set; }

        public int IsDeleted { get; private set; }

        public string? UniqId { get; private set; }

        public string? UniqIdParent { get; private set; }

        public string MaleStd { get; private set; }

        public string FemaleStd { get; private set; }

        public string MaleStdLow { get; private set; }

        public string FemaleStdLow { get; private set; }

        public string MaleStdHigh { get; private set; }

        public string FemaleStdHigh { get; private set; }

        public string Unit { get; private set; }
    }
}
