using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Ika.Models
{
    public class RaiinInfModel
    {
        public RaiinInf RaiinInf { get; } = null;
        public KaMst KaMst { get; } = null;

        public RaiinInfModel(RaiinInf raiinInf, KaMst kaMst)
        {
            RaiinInf = raiinInf;
            KaMst = kaMst;
        }

        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        public int HpId
        {
            get { return RaiinInf.HpId; }
        }

        /// <summary>
        /// 患者ID
        ///  患者を識別するためのシステム固有の番号
        /// </summary>
        public long PtId
        {
            get { return RaiinInf.PtId; }
        }

        /// <summary>
        /// 診療日
        ///  yyyymmdd 
        /// </summary>
        public int SinDate
        {
            get { return RaiinInf.SinDate; }
        }

        /// <summary>
        /// 来院番号
        /// </summary>
        public long RaiinNo
        {
            get { return RaiinInf.RaiinNo; }
        }

        /// <summary>
        /// 親来院番号
        /// </summary>
        public long OyaRaiinNo
        {
            get { return RaiinInf.OyaRaiinNo; }
        }

        /// <summary>
        /// 状態
        ///  0:予約
        ///  1:受付
        ///  3:一時保存
        ///  5:計算
        ///  7:精算待ち
        ///  9:精算済
        /// </summary>
        public int Status
        {
            get { return RaiinInf.Status; }
        }

        /// <summary>
        /// 受付時間
        ///  HH24MISS
        /// </summary>
        public string UketukeTime
        {
            get { return RaiinInf.UketukeTime ?? string.Empty; }
        }

        /// <summary>
        /// 診察開始時間
        ///  HH24MISS
        /// </summary>
        public string SinStartTime
        {
            get { return RaiinInf.SinStartTime ?? string.Empty; }
        }

        /// <summary>
        /// 診療科ID
        /// </summary>
        public int KaId
        {
            get { return RaiinInf.KaId; }
        }

        /// <summary>
        /// 担当医ID
        /// </summary>
        public int TantoId
        {
            get { return RaiinInf.TantoId; }
        }

        /// <summary>
        /// 削除区分
        ///  1: 削除
        /// </summary>
        public int IsDeleted
        {
            get { return RaiinInf.IsDeleted; }
        }

        /// <summary>
        /// 診療科名
        /// </summary>
        public string KaName
        {
            get { return KaMst?.KaName ?? ""; }
        }
    }

}
