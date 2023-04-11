using Entity.Tenant;

namespace Reporting.Sijisen.Model
{
    public class CoRsvkrtOdrInfModel
    {
        public RsvkrtOdrInf RsvkrtOdrInf { get; } = new();

        public CoRsvkrtOdrInfModel(RsvkrtOdrInf rsvkrtOdrInf)
        {
            RsvkrtOdrInf = rsvkrtOdrInf;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return RsvkrtOdrInf.HpId; }
        }

        /// <summary>
        /// 患者ID
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        public long PtId
        {
            get { return RsvkrtOdrInf.PtId; }
        }

        /// <summary>
        /// 予約日
        /// yyyymmdd
        /// </summary>
        public int RsvDate
        {
            get { return RsvkrtOdrInf.RsvDate; }
        }

        /// <summary>
        /// 予約カルテ番号
        /// 
        /// </summary>
        public long RsvkrtNo
        {
            get { return RsvkrtOdrInf.RsvkrtNo; }
        }

        /// <summary>
        /// 剤番号
        /// 
        /// </summary>
        public long RpNo
        {
            get { return RsvkrtOdrInf.RpNo; }
        }

        /// <summary>
        /// 剤枝番
        /// 剤に変更があった場合、カウントアップ
        /// </summary>
        public long RpEdaNo
        {
            get { return RsvkrtOdrInf.RpEdaNo; }
        }

        /// <summary>
        /// ID
        /// </summary>
        public long Id
        {
            get { return RsvkrtOdrInf.Id; }
        }

        /// <summary>
        /// オーダー行為区分
        /// 
        /// </summary>
        public int OdrKouiKbn
        {
            get { return RsvkrtOdrInf.OdrKouiKbn; }
        }

        /// <summary>
        /// 剤名称
        /// 
        /// </summary>
        public string RpName
        {
            get { return RsvkrtOdrInf.RpName ?? string.Empty; }
        }

        /// <summary>
        /// 院内院外区分
        /// "0: 院内
        /// 1: 院外"
        /// </summary>
        public int InoutKbn
        {
            get { return RsvkrtOdrInf.InoutKbn; }
        }

        /// <summary>
        /// 至急区分
        /// "0:通常 
        /// 1:至急"
        /// </summary>
        public int SikyuKbn
        {
            get { return RsvkrtOdrInf.SikyuKbn; }
        }

        /// <summary>
        /// 処方種別
        /// "0: 日数判断
        /// 1: 臨時
        /// 2: 常態"
        /// </summary>
        public int SyohoSbt
        {
            get { return RsvkrtOdrInf.SyohoSbt; }
        }

        /// <summary>
        /// 算定区分
        /// "1: 算定外
        /// 2: 自費算定"
        /// </summary>
        public int SanteiKbn
        {
            get { return RsvkrtOdrInf.SanteiKbn; }
        }

        /// <summary>
        /// 透析区分
        /// "0: 透析以外
        /// 1: 透析前
        /// 2: 透析後"
        /// </summary>
        public int TosekiKbn
        {
            get { return RsvkrtOdrInf.TosekiKbn; }
        }

        /// <summary>
        /// 日数回数
        /// 処方日数
        /// </summary>
        public int DaysCnt
        {
            get { return RsvkrtOdrInf.DaysCnt; }
        }

        /// <summary>
        /// 削除区分
        /// "1: 削除
        /// 2: 実施"
        /// </summary>
        public int IsDeleted
        {
            get { return RsvkrtOdrInf.IsDeleted; }
        }

        /// <summary>
        /// 並び順
        /// 
        /// </summary>
        public int SortNo
        {
            get { return RsvkrtOdrInf.SortNo; }
        }
    }
}
