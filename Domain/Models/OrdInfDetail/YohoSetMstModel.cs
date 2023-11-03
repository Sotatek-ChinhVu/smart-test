namespace Domain.Models.OrdInfDetails
{
    public class YohoSetMstModel
    {
        public YohoSetMstModel(string itemname, int yohoKbn, int setId, int userId, string itemCd)
        {
            Itemname = itemname;
            YohoKbn = yohoKbn;
            SetId = setId;
            UserId = userId;
            ItemCd = itemCd;
            CreateMachine = string.Empty;
            UpdateMachine = string.Empty;
        }

        public YohoSetMstModel(int hpId, int setId, int userId, int sortNo, string itemCd, int isDeleted, DateTime createDate, int createId, string createMachine, DateTime updateDate, int updateId, string updateMachine, string itemName, bool isModified)
        {
            HpId = hpId;
            SetId = setId;
            UserId = userId;
            SortNo = sortNo;
            ItemCd = itemCd;
            IsDeleted = isDeleted;
            CreateDate = createDate;
            CreateId = createId;
            CreateMachine = createMachine;
            UpdateDate = updateDate;
            UpdateId = updateId;
            UpdateMachine = updateMachine;
            Itemname = itemName;
            IsModified = isModified;
        }
        public YohoSetMstModel(int hpId, int setId, int userId, int sortNo, string itemCd, int isDeleted, string itemName, bool isModified)
        {
            HpId = hpId;
            SetId = setId;
            UserId = userId;
            SortNo = sortNo;
            ItemCd = itemCd;
            IsDeleted = isDeleted;
            Itemname = itemName;
            IsModified = isModified;
            CreateMachine = string.Empty;
            UpdateMachine = string.Empty;
        }

        public string Itemname { get; private set; }
        public int YohoKbn { get; private set; }
        public int SetId { get; private set; }
        public int UserId { get; private set; }
        public string ItemCd { get; private set; }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId { get; private set; }
        /// <summary>
        /// 並び順
        /// 
        /// </summary>
        public int SortNo { get; private set; }

        /// <summary>
        /// 削除区分
        /// 1:削除
        /// </summary>
        public int IsDeleted { get; private set; }

        /// <summary>
        /// 作成日時 
        /// </summary>
        public DateTime CreateDate { get; private set; }

        /// <summary>
        /// 作成者  
        /// </summary>
        public int CreateId { get; private set; }

        /// <summary>
        /// 作成端末   
        /// </summary>
        public string CreateMachine { get; private set; }

        /// <summary>
        /// 更新日時   
        /// </summary>
        public DateTime UpdateDate { get; private set; }

        /// <summary>
        /// 更新者   
        /// </summary>
        public int UpdateId { get; private set; }

        /// <summary>
        /// 更新端末   
        /// </summary>
        public string UpdateMachine { get; private set; }


        public bool IsModified { get; private set; }

        public bool CheckDefaultValue()
        {
            return string.IsNullOrEmpty(Itemname);
        }

        public bool CheckDefaultModel()
        {
            return HpId == 0 &&
                   string.IsNullOrEmpty(Itemname);
        }
    }
}
