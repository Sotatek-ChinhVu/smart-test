using Entity.Tenant;
using EmrCalculateApi.Ika.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Receipt.Models
{
    public class UnitMstModel
    {
        public UnitMst UnitMst { get; } = null;

        public UnitMstModel(UnitMst unitMst)
        {
            UnitMst = unitMst;
        }

        /// <summary>
        /// 単位コード
        /// 
        /// </summary>
        public int UnitCd
        {
            get { return UnitMst?.UnitCd ?? 0; }
        }

        /// <summary>
        /// 単位名称
        /// 
        /// </summary>
        public string UnitName
        {
            get { return UnitMst.UnitName ?? string.Empty; }
        }

        /// <summary>
        /// 使用開始日
        /// 
        /// </summary>
        public int StartDate
        {
            get { return UnitMst.StartDate; }
        }

        /// <summary>
        /// 使用終了日
        /// 
        /// </summary>
        public int EndDate
        {
            get { return UnitMst.EndDate; }
        }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        public DateTime CreateDate
        {
            get { return UnitMst.CreateDate; }
        }

        /// <summary>
        /// 作成者ID
        /// 
        /// </summary>
        public int CreateId
        {
            get { return UnitMst.CreateId; }
        }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        public string CreateMachine
        {
            get { return UnitMst.CreateMachine ?? string.Empty; }
        }

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        public DateTime UpdateDate
        {
            get { return UnitMst.UpdateDate; }
        }

        /// <summary>
        /// 更新者ID
        /// 
        /// </summary>
        public int UpdateId
        {
            get { return UnitMst.UpdateId; }
        }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        public string UpdateMachine
        {
            get { return UnitMst.UpdateMachine ?? string.Empty; }
        }
    }

}
