namespace Domain.Models.KensaSetDetail
{
    public class KensaSetDetailModel
    {
        public KensaSetDetailModel(int hpId, int setId, int setEdaNo, string kensaItemCd, string kensaName, int kensaItemSeqNo, int sortNo, int isDeleted)
        {
            HpId = hpId;
            SetId = setId;
            SetEdaNo = setEdaNo;
            KensaItemCd = kensaItemCd;
            KensaName = kensaName;
            KensaItemSeqNo = kensaItemSeqNo;
            SortNo = sortNo;
            IsDeleted = isDeleted;
        }

        public int HpId { get; set; }
        public int SetId { get; set; }
        public int SetEdaNo { get; set; }
        public string KensaItemCd { get; set; }
        public string KensaName { get; set; }
        public int KensaItemSeqNo { get; set; }
        public int SortNo { get; set; }
        public int IsDeleted { get; set; }
    }
}
