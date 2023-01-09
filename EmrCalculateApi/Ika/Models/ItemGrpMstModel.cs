using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Ika.Models
{
    public class ItemGrpMstModel
    {
        public ItemGrpMst ItemGrpMst { get; } = null;

        public ItemGrpMstModel(ItemGrpMst itemGrpMst)
        {
            ItemGrpMst = itemGrpMst;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return ItemGrpMst.HpId; }
        }

        /// <summary>
        /// グループ種別
        /// 1: DENSI_SANTEI_KAISU
        /// </summary>
        public long GrpSbt
        {
            get { return ItemGrpMst.GrpSbt; }
        }

        /// <summary>
        /// 項目グループコード
        /// 
        /// </summary>
        public long ItemGrpCd
        {
            get { return ItemGrpMst.ItemGrpCd; }
        }

        /// <summary>
        /// 開始日
        /// 
        /// </summary>
        public int StartDate
        {
            get { return ItemGrpMst.StartDate; }
        }

        /// <summary>
        /// 終了日
        /// 
        /// </summary>
        public int EndDate
        {
            get { return ItemGrpMst.EndDate; }
        }

        /// <summary>
        /// 診療行為コード
        /// 
        /// </summary>
        public string ItemCd
        {
            get { return ItemGrpMst.ItemCd; }
        }

        /// <summary>
        /// 連番
        /// 同一グループ種別、項目グループコード、開始日内の連番
        /// </summary>
        public int SeqNo
        {
            get { return ItemGrpMst.SeqNo; }
        }

        ///// <summary>
        ///// 作成日時
        ///// 
        ///// </summary>
        //public DateTime CreateDate
        //{
        //    get { return ItemGrpMst.CreateDate; }
        //}

        ///// <summary>
        ///// 作成者
        ///// 
        ///// </summary>
        //public int CreateId
        //{
        //    get { return ItemGrpMst.CreateId; }
        //}

        ///// <summary>
        ///// 作成端末
        ///// 
        ///// </summary>
        //public string CreateMachine
        //{
        //    get { return ItemGrpMst.CreateMachine; }
        //}

        ///// <summary>
        ///// 更新日時
        ///// 
        ///// </summary>
        //public DateTime UpdateDate
        //{
        //    get { return ItemGrpMst.UpdateDate; }
        //}

        ///// <summary>
        ///// 更新者
        ///// 
        ///// </summary>
        //public int UpdateId
        //{
        //    get { return ItemGrpMst.UpdateId; }
        //}

        ///// <summary>
        ///// 更新端末
        ///// 
        ///// </summary>
        //public string UpdateMachine
        //{
        //    get { return ItemGrpMst.UpdateMachine; }
        //}


    }

}
