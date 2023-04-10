namespace Reporting.Sijisen.Model
{
    public class CoCommonOdrInfDetailModel
    {
        public CoCommonOdrInfDetailModel(
            int hpId, long ptId, int sinDate, long raiinNo,
            long rpNo, long rpEdaNo, int rowNo,
            int sinKouiKbn,
            string itemCd, string itemName,
            double suryo, string unitName, string bunkatu,
            string materialName, string containerName)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            RpNo = rpNo;
            RpEdaNo = rpEdaNo;
            RowNo = rowNo;
            SinKouiKbn = sinKouiKbn;
            ItemCd = itemCd;
            ItemName = itemName;
            Suryo = suryo;
            UnitName = unitName;
            Bunkatu = bunkatu;
            MaterialName = materialName;
            ContainerName = containerName;
        }
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        ///       患者を識別するためのシステム固有の番号
        /// </summary>
        public long PtId { get; set; }

        /// <summary>
        /// 診療日
        ///       yyyyMMdd
        /// </summary>
        public int SinDate { get; set; }

        /// <summary>
        /// 来院番号
        /// </summary>
        public long RaiinNo { get; set; }

        /// <summary>
        /// 剤番号
        ///     ODR_INF.RP_NO
        /// </summary>
        public long RpNo { get; set; }

        /// <summary>
        /// 剤枝番
        ///     ODR_INF.RP_EDA_NO
        /// </summary>
        public long RpEdaNo { get; set; }

        /// <summary>
        /// 行番号
        /// </summary>
        public int RowNo { get; set; }

        /// <summary>
        /// 診療行為区分
        /// </summary>
        public int SinKouiKbn { get; set; }

        /// <summary>
        /// 項目コード
        /// </summary>
        public string ItemCd { get; set; }

        /// <summary>
        /// 項目名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public double Suryo { get; set; }

        /// <summary>
        /// 単位名称
        /// </summary>
        public string UnitName { get; set; }

        /// <summary>
        /// 分割調剤
        ///        7日単位の3分割の場合 "7+7+7"
        /// </summary>
        public string Bunkatu { get; set; }

        /// <summary>
        /// 材料名称
        /// </summary>
        public string MaterialName { get; set; }
        /// <summary>
        /// 容器名称
        /// </summary>
        public string ContainerName { get; set; }
    }
}
