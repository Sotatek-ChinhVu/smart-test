namespace Domain.Models.KensaSetDetail
{
    public class KensaSetDetailModel
    {
        public KensaSetDetailModel(int hpId, int setId, int setEdaNo, string kensaItemCd, int kensaItemSeqNo, int sortNo, int isDeleted)
        {
            HpId = hpId;
            SetId = setId;
            SetEdaNo = setEdaNo;
            KensaItemCd = kensaItemCd;
            KensaItemSeqNo = kensaItemSeqNo;
            SortNo = sortNo;
            IsDeleted = isDeleted;
        }

        public int HpId { get; private set; }

        public int SetId { get; private set; }

        public int SetEdaNo { get; private set; }

        public string KensaItemCd { get; private set; }

        public int KensaItemSeqNo { get; private set; }

        public int SortNo { get; private set; }

        public int IsDeleted { get; private set; }
    }
}
