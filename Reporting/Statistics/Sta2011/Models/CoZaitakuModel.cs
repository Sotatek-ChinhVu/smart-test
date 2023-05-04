namespace Reporting.Statistics.Sta2011.Models
{
    public class CoZaitakuModel
    {
        public CoZaitakuModel(int seikyuYm, long ptId, int sinYm, int hokenId)
        {
            SeikyuYm = seikyuYm;
            PtId = ptId;
            SinYm = sinYm;
            HokenId = hokenId;
        }

        /// <summary>
        /// 請求年月
        /// </summary>
        public int SeikyuYm { get; private set; }

        /// <summary>
        /// 患者ID
        /// </summary>
        public long PtId { get; private set; }

        /// <summary>
        /// 診療年月
        /// </summary>
        public int SinYm { get; private set; }

        /// <summary>
        /// 保険ID
        /// </summary>
        public int HokenId { get; private set; }
    }
}
