using Entity.Tenant;

namespace EmrCalculateApi.Futan.Models
{
    public class SinKouiCountModel
    {
        public SinKouiCount SinKouiCount { get; }

        public SinKouiCountModel(SinKouiCount sinKouiCount)
        {
            SinKouiCount = sinKouiCount;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return SinKouiCount.HpId; }
        }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        public long PtId
        {
            get { return SinKouiCount.PtId; }
        }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        public int SinYm
        {
            get { return SinKouiCount.SinYm; }
        }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        public int SinDay
        {
            get { return SinKouiCount.SinDay; }
        }

        /// <summary>
        /// 診療年月
        /// </summary>
        public int SinDate
        {
            get { return SinKouiCount.SinDate; }
        }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        public long RaiinNo
        {
            get { return SinKouiCount.RaiinNo; }
        }

        /// <summary>
        /// 剤番号
        /// SEQUENCE SIN_KOUI.RP_NO
        /// </summary>
        public int RpNo
        {
            get { return SinKouiCount.RpNo; }
        }

        /// <summary>
        /// 連番
        /// SIN_KOUI.SEQ_NO
        /// </summary>
        public int SeqNo
        {
            get { return SinKouiCount.SeqNo; }
        }

        /// <summary>
        /// 回数
        /// 来院ごとの回数
        /// </summary>
        public int Count
        {
            get { return SinKouiCount.Count; }
        }
    }
}
