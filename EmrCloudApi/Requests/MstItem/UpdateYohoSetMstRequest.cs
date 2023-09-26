using Domain.Models.OrdInfDetails;

namespace EmrCloudApi.Requests.MstItem
{
    public class UpdateYohoSetMstRequest
    {
        public List<YohoSetMstRequest> YohoSetMsts { get; set; }
    }
    public class YohoSetMstRequest
    {
        public string Itemname { get; set; }
        public int YohoKbn { get; set; }
        public int SetId { get; set; }
        public int UserId { get; set; }
        public string ItemCd { get; set; }

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

        /// <summary>
        /// 作成日時 
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者  
        /// </summary>
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末   
        /// </summary>
        public string CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時   
        /// </summary>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者   
        /// </summary>
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末   
        /// </summary>
        public string UpdateMachine { get; set; } = string.Empty;


        public bool IsModified { get; set; }
    }
}
