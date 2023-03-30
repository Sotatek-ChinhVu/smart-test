using Entity.Tenant;

namespace Reporting.Karte1.Model
{
    public class CoPtKohiModel
    {
        public PtKohi PtKohi { get; } = new();
        public HokenMst HokenMst { get; } = new();
        public KohiPriority KohiPriority { get; } = new();

        public CoPtKohiModel(PtKohi ptKohi, HokenMst hokenMst)
        {
            PtKohi = ptKohi;
            HokenMst = hokenMst;
        }

        /// <summary>
        /// 患者公費情報
        /// </summary>
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        public int HpId
        {
            get { return PtKohi.HpId; }
        }

        /// <summary>
        /// 患者ID
        ///  患者を識別するためのシステム固有の番号
        /// </summary>
        public long PtId
        {
            get { return PtKohi.PtId; }
        }

        /// <summary>
        /// 保険ID
        ///  患者別に保険情報を識別するための固有の番号
        /// </summary>
        public int HokenId
        {
            get { return PtKohi.HokenId; }
        }

        /// <summary>
        /// 連番
        /// </summary>
        public long SeqNo
        {
            get { return PtKohi.SeqNo; }
        }

        /// <summary>
        /// 都道府県番号
        ///  保険マスタの都道府県番号
        /// </summary>
        public int PrefNo
        {
            get { return PtKohi.PrefNo; }
        }

        /// <summary>
        /// 保険番号
        ///  保険マスタに登録された保険番号
        /// </summary>
        public int HokenNo
        {
            get { return PtKohi.HokenNo; }
        }

        /// <summary>
        /// 保険番号枝番
        ///  保険マスタに登録された保険番号枝番
        /// </summary>
        public int HokenEdaNo
        {
            get { return PtKohi.HokenEdaNo; }
        }

        /// <summary>
        /// 負担者番号
        /// </summary>
        public string FutansyaNo
        {
            get { return PtKohi.FutansyaNo ?? ""; }
        }

        /// <summary>
        /// 受給者番号
        /// </summary>
        public string JyukyusyaNo
        {
            get { return PtKohi.JyukyusyaNo ?? ""; }
        }

        /// <summary>
        /// 特殊受給者番号
        /// </summary>
        public string TokusyuNo
        {
            get { return PtKohi.TokusyuNo ?? ""; }
        }

        /// <summary>
        /// 資格取得日
        ///  yyyymmdd 
        /// </summary>
        public int SikakuDate
        {
            get { return PtKohi.SikakuDate; }
        }

        /// <summary>
        /// 交付日
        ///  yyyymmdd
        /// </summary>
        public int KofuDate
        {
            get { return PtKohi.KofuDate; }
        }

        /// <summary>
        /// 適用開始日
        ///  yyyymmdd
        /// </summary>
        public int StartDate
        {
            get { return PtKohi.StartDate; }
        }

        /// <summary>
        /// 適用終了日
        ///  yyyymmdd
        /// </summary>
        public int EndDate
        {
            get { return PtKohi.EndDate; }
        }

        /// <summary>
        /// 負担率
        ///  yyyymmdd
        /// </summary>
        public int Rate
        {
            get { return PtKohi.Rate; }
        }

        /// <summary>
        /// 一部負担限度額
        ///  yyyymmdd
        /// </summary>
        public int GendoGaku
        {
            get { return PtKohi.GendoGaku; }
        }

        /// <summary>
        /// 削除区分
        ///  1:削除
        /// </summary>
        public int IsDeleted
        {
            get { return PtKohi.IsDeleted; }
        }

        /// <summary>
        /// 保険種別区分
        /// 0:保険なし 1:主保険   2:マル長   3:労災  4:自賠
        /// 5:生活保護 6:分点公費 7:一般公費  8:自費 9:自費レセ
        /// </summary>
        public int HokenSbtKbn
        {
            get => HokenMst.HokenSbtKbn;
        }
        /// <summary>
        /// 負担区分
        /// </summary>
        public int FutanKbn
        {
            get => HokenMst.FutanKbn;
        }
        /// <summary>
        /// 負担率
        /// </summary>
        public int FutanRate
        {
            get => HokenMst.FutanRate;
        }
        /// <summary>
        /// 月上限額
        /// </summary>
        public int MonthLimitFutan
        {
            get => HokenMst.MonthLimitFutan;
        }
        /// <summary>
        /// 日上限
        /// </summary>
        public int DayLimitFutan
        {
            get => HokenMst.DayLimitFutan;
        }
    }

}
