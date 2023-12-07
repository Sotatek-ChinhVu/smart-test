using Entity.Tenant;

namespace Reporting.Yakutai.Model
{
    public class CoRaiinInfModel
    {
        public RaiinInf RaiinInf { get; set; }
        public KaMst KaMst { get; set; }
        public UserMst UserMst { get; set; }

        public CoRaiinInfModel(RaiinInf raiinInf, KaMst kaMst, UserMst userMst)
        {
            RaiinInf = raiinInf;
            KaMst = kaMst;
            UserMst = userMst;
        }

        public CoRaiinInfModel()
        {
            RaiinInf = new();
            KaMst = new();
            UserMst = new();
        }

        /// <summary>
        /// 来院情報
        /// </summary>
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
        /// 予約フラグ
        ///  1:予約の来院  
        /// </summary>
        public int IsYoyaku
        {
            get { return RaiinInf.IsYoyaku; }
        }

        /// <summary>
        /// 予約時間
        ///  HH24MISS
        /// </summary>
        public string YoyakuTime
        {
            get { return RaiinInf.YoyakuTime ?? string.Empty; }
        }

        /// <summary>
        /// 予約者ID
        /// </summary>
        public int YoyakuId
        {
            get { return RaiinInf.YoyakuId; }
        }

        /// <summary>
        /// 受付種別
        /// </summary>
        public int UketukeSbt
        {
            get { return RaiinInf.UketukeSbt; }
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
        /// 受付者ID
        /// </summary>
        public int UketukeId
        {
            get { return RaiinInf.UketukeId; }
        }

        /// <summary>
        /// 受付番号
        /// </summary>
        public int UketukeNo
        {
            get { return RaiinInf.UketukeNo; }
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
        /// 診察終了時間
        ///  HH24MISS　※状態が計算以上になった時間        
        /// </summary>
        public string SinEndTime
        {
            get { return RaiinInf.SinEndTime ?? string.Empty; }
        }

        /// <summary>
        /// 精算時間
        ///  HH24MISS
        /// </summary>
        public string KaikeiTime
        {
            get { return RaiinInf.KaikeiTime ?? string.Empty; }
        }

        /// <summary>
        /// 精算者ID
        /// </summary>
        public int KaikeiId
        {
            get { return RaiinInf.KaikeiId; }
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
        /// 保険組合せID
        ///  患者別に保険情報を識別するための固有の番号
        /// </summary>
        public int HokenPid
        {
            get { return RaiinInf.HokenPid; }
        }

        /// <summary>
        /// 初再診区分
        ///  受付時設定、ODR_INF更新後はトリガーで設定       
        /// </summary>
        public int SyosaisinKbn
        {
            get { return RaiinInf.SyosaisinKbn; }
        }

        /// <summary>
        /// 時間枠区分
        ///  受付時設定、ODR_INF更新後はトリガーで設定       
        /// </summary>
        public int JikanKbn
        {
            get { return RaiinInf.JikanKbn; }
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
            get { return KaMst != null ? (KaMst.KaName ?? "") : ""; }
        }
        /// <summary>
        /// 担当医氏名
        /// </summary>
        public string TantoName
        {
            get { return UserMst != null ? (UserMst.Name ?? "") : ""; }
        }
    }
}
