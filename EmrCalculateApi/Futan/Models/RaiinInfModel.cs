using Entity.Tenant;

namespace EmrCalculateApi.Futan.Models
{
    public class RaiinInfModel
    {
        public RaiinInf RaiinInf { get; }

        public RaiinInfModel(RaiinInf raiinInf)
        {
            RaiinInf = raiinInf;
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
        /// 診察開始時間
        ///  HH24MISS
        /// </summary>
        public string SinStartTime
        {
            get { return RaiinInf.SinStartTime ?? string.Empty; }
        }

        /// <summary>
        /// 初再診区分
        ///  受付時設定、ODR_INF更新後はトリガーで設定       
        /// </summary>
        public int SyosaisinKbn
        {
            get { return RaiinInf.SyosaisinKbn; }
        }
    }
}
