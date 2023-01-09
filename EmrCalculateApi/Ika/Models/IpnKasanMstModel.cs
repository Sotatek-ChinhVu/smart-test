using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Ika.Models
{
    public class IpnKasanMstModel 
    {
        public IpnKasanMst IpnKasanMst { get; } = null;

        public IpnKasanMstModel(IpnKasanMst ipnKasanMst)
        {
            IpnKasanMst = ipnKasanMst;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return IpnKasanMst.HpId; }
            set
            {
                if (IpnKasanMst.HpId == value) return;
                IpnKasanMst.HpId = value;
                //RaisePropertyChanged(() => HpId);
            }
        }

        /// <summary>
        /// 一般名コード
        /// 
        /// </summary>
        public string IpnNameCd
        {
            get { return IpnKasanMst.IpnNameCd ?? string.Empty; }
        }

        /// <summary>
        /// 開始日
        /// 
        /// </summary>
        public int StartDate
        {
            get { return IpnKasanMst.StartDate; }
        }

        /// <summary>
        /// 終了日
        /// 
        /// </summary>
        public int EndDate
        {
            get { return IpnKasanMst.EndDate; }
        }

        /// <summary>
        /// 加算１
        /// 1: 一般名処方加算１の対象
        /// </summary>
        public int Kasan1
        {
            get { return IpnKasanMst.Kasan1; }
        }

        /// <summary>
        /// 加算２
        /// 1: 一般名処方加算２の対象
        /// </summary>
        public int Kasan2
        {
            get { return IpnKasanMst.Kasan2; }
        }

        /// <summary>
        /// 連番
        /// 同一一般名コード、開始日内の連番
        /// </summary>
        public int SeqNo
        {
            get { return IpnKasanMst.SeqNo; }
        }

        /// <summary>
        /// 削除区分
        /// 1:削除
        /// </summary>
        public int IsDeleted
        {
            get { return IpnKasanMst.IsDeleted; }
        }

        ///// <summary>
        ///// 作成日時
        ///// 
        ///// </summary>
        //public DateTime CreateDate
        //{
        //    get { return IpnKasanMst.CreateDate; }
        //}

        ///// <summary>
        ///// 作成者
        ///// 
        ///// </summary>
        //public int CreateId
        //{
        //    get { return IpnKasanMst.CreateId; }
        //}

        ///// <summary>
        ///// 作成端末
        ///// 
        ///// </summary>
        //public string CreateMachine
        //{
        //    get { return IpnKasanMst.CreateMachine; }
        //}

        ///// <summary>
        ///// 更新日時
        ///// 
        ///// </summary>
        //public DateTime UpdateDate
        //{
        //    get { return IpnKasanMst.UpdateDate; }
        //}

        ///// <summary>
        ///// 更新者
        ///// 
        ///// </summary>
        //public int UpdateId
        //{
        //    get { return IpnKasanMst.UpdateId; }
        //}

        ///// <summary>
        ///// 更新端末
        ///// 
        ///// </summary>
        //public string UpdateMachine
        //{
        //    get { return IpnKasanMst.UpdateMachine; }
        //}


    }

}
