using Entity.Tenant;

namespace Reporting.Yakutai.Model
{
    public class CoYohoInfMstModel
    {
        public YohoInfMst YohoInfMst { get; } = null;

        public CoYohoInfMstModel(YohoInfMst yohoInfMst)
        {
            YohoInfMst = yohoInfMst;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return YohoInfMst.HpId; }
        }

        /// <summary>
        /// 診療行為コード
        /// TEN_MST.ITEM_CD
        /// </summary>
        public string ItemCd
        {
            get { return YohoInfMst.ItemCd; }
        }

        /// <summary>
        /// 用法接尾語
        /// 
        /// </summary>
        public string YohoSuffix
        {
            get { return YohoInfMst.YohoSuffix; }
        }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        public DateTime CreateDate
        {
            get { return YohoInfMst.CreateDate; }
        }

        /// <summary>
        /// 作成者
        /// 
        /// </summary>
        public int CreateId
        {
            get { return YohoInfMst.CreateId; }
        }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        public string CreateMachine
        {
            get { return YohoInfMst.CreateMachine; }
        }

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        public DateTime UpdateDate
        {
            get { return YohoInfMst.UpdateDate; }
        }

        /// <summary>
        /// 更新者
        /// 
        /// </summary>
        public int UpdateId
        {
            get { return YohoInfMst.UpdateId; }
        }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        public string UpdateMachine
        {
            get { return YohoInfMst.UpdateMachine; }
        }


    }
}
