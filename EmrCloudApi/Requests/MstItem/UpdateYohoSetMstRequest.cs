namespace EmrCloudApi.Requests.MstItem
{
    public class UpdateYohoSetMstRequest
    {
        public List<YohoSetMstRequest> YohoSetMsts { get; set; } = new();
    }
    public class YohoSetMstRequest
    {
        public string Itemname { get; set; } = string.Empty;
        public int YohoKbn { get; set; }
        public int SetId { get; set; }
        public int UserId { get; set; }
        public string ItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId { get; set; }
        /// <summary>
        /// 並び順
        /// 
        /// </summary>
        public int SortNo { get; set; }

        /// <summary>
        /// 削除区分
        /// 1:削除
        /// </summary>
        public int IsDeleted { get; set; }

        public bool IsModified { get; set; }
    }
}
