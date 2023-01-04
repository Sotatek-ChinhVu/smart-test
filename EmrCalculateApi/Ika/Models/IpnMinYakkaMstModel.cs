using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Ika.Models
{
    public class IpnMinYakkaMstModel 
    {
        public IpnMinYakkaMst IpnMinYakkaMst { get; } = null;

        public IpnMinYakkaMstModel(IpnMinYakkaMst ipnMinYakkaMst)
        {
            IpnMinYakkaMst = ipnMinYakkaMst;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return IpnMinYakkaMst.HpId; }
        }

        /// <summary>
        /// 一般名コード
        /// 
        /// </summary>
        public string IpnNameCd
        {
            get { return IpnMinYakkaMst.IpnNameCd ?? string.Empty; }
        }

        /// <summary>
        /// 開始日
        /// 
        /// </summary>
        public int StartDate
        {
            get { return IpnMinYakkaMst.StartDate; }
        }

        /// <summary>
        /// 終了日
        /// 
        /// </summary>
        public int EndDate
        {
            get { return IpnMinYakkaMst.EndDate; }
        }

        /// <summary>
        /// 最低薬価
        /// 
        /// </summary>
        public double Yakka
        {
            get { return IpnMinYakkaMst.Yakka; }
        }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        public int SeqNo
        {
            get { return IpnMinYakkaMst.SeqNo; }
        }

        /// <summary>
        /// 削除区分
        /// 同一一般名コード、開始日内の連番
        /// </summary>
        public int IsDeleted
        {
            get { return IpnMinYakkaMst.IsDeleted; }
        }

        ///// <summary>
        ///// 作成日時
        ///// 
        ///// </summary>
        //public DateTime CreateDate
        //{
        //    get { return IpnMinYakkaMst.CreateDate; }
        //}

        ///// <summary>
        ///// 作成者
        ///// 
        ///// </summary>
        //public int CreateId
        //{
        //    get { return IpnMinYakkaMst.CreateId; }
        //}

        ///// <summary>
        ///// 作成端末
        ///// 
        ///// </summary>
        //public string CreateMachine
        //{
        //    get { return IpnMinYakkaMst.CreateMachine; }
        //}

        ///// <summary>
        ///// 更新日時
        ///// 
        ///// </summary>
        //public DateTime UpdateDate
        //{
        //    get { return IpnMinYakkaMst.UpdateDate; }
        //}

        ///// <summary>
        ///// 更新者
        ///// 
        ///// </summary>
        //public int UpdateId
        //{
        //    get { return IpnMinYakkaMst.UpdateId; }
        //}

        ///// <summary>
        ///// 更新端末
        ///// 
        ///// </summary>
        //public string UpdateMachine
        //{
        //    get { return IpnMinYakkaMst.UpdateMachine; }
        //}


    }

}
