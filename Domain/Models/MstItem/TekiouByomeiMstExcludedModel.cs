namespace Domain.Models.MstItem
{
    public class TekiouByomeiMstExcludedModel
    {
        public TekiouByomeiMstExcludedModel(int hpId, string itemCd, int seqNo, int isDeleted)
        {
            HpId = hpId;
            ItemCd = itemCd;
            SeqNo = seqNo;
            IsDeleted = isDeleted;
        }


        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId { get; private set; }

        /// <summary>
        /// 項目コード
        /// 
        /// </summary>
        public string ItemCd { get; private set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        public int SeqNo { get; private set; }

        /// <summary>
        /// 削除区分
        /// 1: 削除
        /// </summary>
        public int IsDeleted { get; private set; }
    }
}
