using Entity.Tenant;

namespace Reporting.InDrug.Model
{
    public class CoOdrInfModel
    {
        public OdrInf OdrInf { get; } = new();

        public CoOdrInfModel(OdrInf odrInf)
        {
            OdrInf = odrInf;
        }

        /// <summary>
        /// オーダー情報
        /// </summary>
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        public int HpId
        {
            get { return OdrInf.HpId; }
        }

        /// <summary>
        /// 患者ID
        ///     患者を識別するためのシステム固有の番号
        /// </summary>
        public long PtId
        {
            get { return OdrInf.PtId; }
        }

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
        /// 剤番号
        /// </summary>
        public long RpNo
        {
            get { return OdrInf.RpNo; }
        }

        /// <summary>
        /// 剤枝番
        ///     剤に変更があった場合、カウントアップ
        /// </summary>
        public long RpEdaNo
        {
            get { return OdrInf.RpEdaNo; }
        }

        /// <summary>
        /// ID
        /// </summary>
        public long Id
        {
            get { return OdrInf.Id; }
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
        /// 剤名称
        /// </summary>
        public string RpName
        {
            get { return OdrInf.RpName ?? ""; }
        }

        /// <summary>
        /// 院内院外区分
        ///     0: 院内
        ///     1: 院外
        /// </summary>
        public int InoutKbn
        {
            get { return OdrInf.InoutKbn; }
        }

        /// <summary>
        /// 至急区分
        ///     0: 通常 
        ///     1: 至急
        /// </summary>
        public int SikyuKbn
        {
            get { return OdrInf.SikyuKbn; }
        }

        /// <summary>
        /// 処方種別
        ///     0: 日数判断
        ///     1: 臨時
        ///     2: 常態
        /// </summary>
        public int SyohoSbt
        {
            get { return OdrInf.SyohoSbt; }
        }

        /// <summary>
        /// 算定区分
        ///     1: 算定外
        ///     2: 自費算定
        /// </summary>
        public int SanteiKbn
        {
            get { return OdrInf.SanteiKbn; }
        }

        /// <summary>
        /// 透析区分
        ///     0: 透析以外
        ///     1: 透析前
        ///     2: 透析後
        /// </summary>
        public int TosekiKbn
        {
            get { return OdrInf.TosekiKbn; }
        }

        /// <summary>
        /// 日数回数
        ///     処方日数
        /// </summary>
        public int DaysCnt
        {
            get { return OdrInf.DaysCnt; }
        }

        /// <summary>
        /// 並び順
        /// </summary>
        public int SortNo
        {
            get { return OdrInf.SortNo; }
        }

        /// <summary>
        /// 削除区分
        ///     1:削除
        ///     2:未確定削除
        /// </summary>
        public int IsDeleted
        {
            get { return OdrInf.IsDeleted; }
        }
    }
}
