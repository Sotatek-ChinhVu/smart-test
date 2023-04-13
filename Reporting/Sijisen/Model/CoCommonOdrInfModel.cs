namespace Reporting.Sijisen.Model
{
    public class CoCommonOdrInfModel
    {
        public CoCommonOdrInfModel(
            int hpId, long ptId, int sinDate, long raiinNo,
            long rpNo, long rpEdaNo,
            int odrKouiKbn, string rpName,
            int inoutKbn, int sikyuKbn, int syohoSbt, int santeiKbn, int tosekiKbn, int daysCnt, int sortNo)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            RpNo = rpNo;
            RpEdaNo = rpEdaNo;
            OdrKouiKbn = odrKouiKbn;
            RpName = rpName;
            InoutKbn = inoutKbn;
            SikyuKbn = sikyuKbn;
            SyohoSbt = syohoSbt;
            SanteiKbn = santeiKbn;
            TosekiKbn = tosekiKbn;
            DaysCnt = daysCnt;
            SortNo = sortNo;
        }

        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        ///     患者を識別するためのシステム固有の番号
        /// </summary>
        public long PtId { get; set; }

        /// <summary>
        /// 診療日
        ///     yyyymmdd
        /// </summary>
        public int SinDate { get; set; }

        /// <summary>
        /// 来院番号
        /// </summary>
        public long RaiinNo { get; set; }

        /// <summary>
        /// 剤番号
        /// </summary>
        public long RpNo { get; set; }

        /// <summary>
        /// 剤枝番
        ///     剤に変更があった場合、カウントアップ
        /// </summary>
        public long RpEdaNo { get; set; }

        /// <summary>
        /// オーダー行為区分
        /// </summary>
        public int OdrKouiKbn { get; set; }

        /// <summary>
        /// 剤名称
        /// </summary>
        public string RpName { get; set; }

        /// <summary>
        /// 院内院外区分
        ///     0: 院内
        ///     1: 院外
        /// </summary>
        public int InoutKbn { get; set; }

        /// <summary>
        /// 至急区分
        ///     0: 通常 
        ///     1: 至急
        /// </summary>
        public int SikyuKbn { get; set; }

        /// <summary>
        /// 処方種別
        ///     0: 日数判断
        ///     1: 臨時
        ///     2: 常態
        /// </summary>
        public int SyohoSbt { get; set; }

        /// <summary>
        /// 算定区分
        ///     1: 算定外
        ///     2: 自費算定
        /// </summary>
        public int SanteiKbn { get; set; }

        /// <summary>
        /// 透析区分
        ///     0: 透析以外
        ///     1: 透析前
        ///     2: 透析後
        /// </summary>
        public int TosekiKbn { get; set; }

        /// <summary>
        /// 日数回数
        ///     処方日数
        /// </summary>
        public int DaysCnt { get; set; }

        /// <summary>
        /// 並び順
        /// </summary>
        public int SortNo { get; set; }
    }
}
