using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Ika.Models
{
    public class RousaiGoseiMstModel 
    {
        public RousaiGoseiMst RousaiGoseiMst { get; } = null;

        public RousaiGoseiMstModel(RousaiGoseiMst rousaiGoseiMst)
        {
            RousaiGoseiMst = rousaiGoseiMst;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return RousaiGoseiMst.HpId; }
        }

        /// <summary>
        /// 合成グループ
        /// 
        /// </summary>
        public int GoseiGrp
        {
            get { return RousaiGoseiMst.GoseiGrp; }
        }

        /// <summary>
        /// 合成項目コード
        /// 
        /// </summary>
        public string GoseiItemCd
        {
            get { return RousaiGoseiMst.GoseiItemCd ?? string.Empty; }
        }

        /// <summary>
        /// 診療行為コード
        /// 
        /// </summary>
        public string ItemCd
        {
            get { return RousaiGoseiMst.ItemCd ?? string.Empty; }
        }

        /// <summary>
        /// 四肢加算区分
        /// 
        /// </summary>
        public int SisiKbn
        {
            get { return RousaiGoseiMst.SisiKbn; }
        }

        /// <summary>
        /// 使用開始日
        /// 
        /// </summary>
        public int StartDate
        {
            get { return RousaiGoseiMst.StartDate; }
        }

        /// <summary>
        /// 使用終了日
        /// 
        /// </summary>
        public int EndDate
        {
            get { return RousaiGoseiMst.EndDate; }
        }

        ///// <summary>
        ///// 作成日時
        ///// 
        ///// </summary>
        //public DateTime CreateDate
        //{
        //    get { return RousaiGoseiMst.CreateDate; }
        //}

        ///// <summary>
        ///// 作成者
        ///// 
        ///// </summary>
        //public int CreateId
        //{
        //    get { return RousaiGoseiMst.CreateId; }
        //}

        ///// <summary>
        ///// 作成端末
        ///// 
        ///// </summary>
        //public string CreateMachine
        //{
        //    get { return RousaiGoseiMst.CreateMachine; }
        //}

        ///// <summary>
        ///// 更新日時
        ///// 
        ///// </summary>
        //public DateTime UpdateDate
        //{
        //    get { return RousaiGoseiMst.UpdateDate; }
        //}

        ///// <summary>
        ///// 更新者
        ///// 
        ///// </summary>
        //public int UpdateId
        //{
        //    get { return RousaiGoseiMst.UpdateId; }
        //}

        ///// <summary>
        ///// 更新端末
        ///// 
        ///// </summary>
        //public string UpdateMachine
        //{
        //    get { return RousaiGoseiMst.UpdateMachine; }
        //}


    }

}
