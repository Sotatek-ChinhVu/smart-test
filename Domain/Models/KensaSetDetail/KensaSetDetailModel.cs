namespace Domain.Models.KensaSetDetail
{
    public class KensaSetDetailModel
    {
        public KensaSetDetailModel(int hpId, int setId, int setEdaNo, string kensaItemCd, string oyaItemCd, string kensaName, int kensaItemSeqNo, int sortNo, List<KensaSetDetailModel> childrens, int isDeleted)
        {
            HpId = hpId;
            SetId = setId;
            SetEdaNo = setEdaNo;
            KensaItemCd = kensaItemCd;
            OyaItemCd = oyaItemCd;
            KensaName = kensaName;
            KensaItemSeqNo = kensaItemSeqNo;
            SortNo = sortNo;
            Childrens = childrens;
            IsDeleted = isDeleted;
        }

        public int HpId { get; private set; }

        public int SetId { get; private set; }

        public int SetEdaNo { get; private set; }

        public string KensaItemCd { get; private set; }

        public string OyaItemCd { get; private set; }

        public string KensaName { get; private set; }

        public int KensaItemSeqNo { get; private set; }

        public int SortNo { get; private set; }

        public List<KensaSetDetailModel> Childrens { get; private set; }

        public int IsDeleted { get; private set; }
    }
}
