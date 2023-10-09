namespace Domain.Models.KensaSetDetail
{
    public class KensaSetDetailModel
    {
        public KensaSetDetailModel(int hpId, int setId, int setEdaNo, string kensaItemCd, string oyaItemCd, string kensaName, int kensaItemSeqNo, int sortNo, List<KensaSetDetailModel>? childrens = null)
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
        }

        public int HpId { get; set; }
        public int SetId { get; set; }
        public int SetEdaNo { get; set; }
        public string KensaItemCd { get; set; }
        public string OyaItemCd { get; set; }
        public string KensaName { get; set; }
        public int KensaItemSeqNo { get; set; }
        public int SortNo { get; set; }
        public int IsDeleted { get; set; }
        public List<KensaSetDetailModel>? Childrens { get; set; }
    }
}
