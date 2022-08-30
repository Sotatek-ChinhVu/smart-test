using Entity.Tenant;

namespace EmrCalculateApi.Futan.Models
{
    public class OdrInfModel
    {
        public OdrInf OdrInf { get; }
        public OdrInfDetail OdrInfDetail { get; }
        public PtHokenPattern PtHokenPattern { get; }

        public OdrInfModel(OdrInf odrInf, OdrInfDetail odrInfDetail, PtHokenPattern ptHokenPattern)
        {
            OdrInf = odrInf;
            OdrInfDetail = odrInfDetail;
            PtHokenPattern = ptHokenPattern;
        }

        /// <summary>
        /// オーダー情報
        /// </summary>
        /// <summary>
        /// 診療日
        ///     yyyymmdd
        /// </summary>
        public int SinDate
        {
            get { return OdrInf.SinDate; }
        }

        /// <summary>
        /// 来院番号
        /// </summary>
        public long RaiinNo
        {
            get { return OdrInf.RaiinNo; }
        }

        /// <summary>
        /// 保険組合せID
        /// </summary>
        public int HokenPid
        {
            get { return OdrInf.HokenPid; }
        }

        /// <summary>
        /// オーダー行為区分
        /// </summary>
        public int OdrKouiKbn
        {
            get { return OdrInf.OdrKouiKbn; }
        }

        /// <summary>
        /// 項目コード
        /// </summary>
        public string ItemCd
        {
            get { return OdrInfDetail.ItemCd ?? string.Empty; }
        }

        /// <summary>
        /// 数量
        /// </summary>
        public double Suryo
        {
            get { return OdrInfDetail.Suryo; }
        }

        /// <summary>
        /// 主保険 保険ID
        /// </summary>
        public int HokenId
        {
            get { return PtHokenPattern.HokenId; }
        }

        /// <summary>
        /// 公費１ 保険ID
        /// </summary>
        public int Kohi1Id
        {
            get { return PtHokenPattern.Kohi1Id; }
        }

        /// <summary>
        /// 公費２ 保険ID
        /// </summary>
        public int Kohi2Id
        {
            get { return PtHokenPattern.Kohi2Id; }
        }

        /// <summary>
        /// 公費３ 保険ID
        /// </summary>
        public int Kohi3Id
        {
            get { return PtHokenPattern.Kohi3Id; }
        }

        /// <summary>
        /// 公費４ 保険ID
        /// </summary>
        public int Kohi4Id
        {
            get { return PtHokenPattern.Kohi4Id; }
        }
    }
}
