using Helper.Common;
using Helper.Extension;
using Reporting.Statistics.Enums;

namespace Reporting.Statistics.Sta9000.Models;

public class CoSta9000PrintData
{
    public CoSta9000PrintData(RowType rowType = RowType.Data)
    {
        RowType = rowType;
    }

    //コピー用メソッド
    public CoSta9000PrintData Clone()
    {
        return (CoSta9000PrintData)MemberwiseClone();
    }

    /// <summary>
    /// 行タイプ
    /// </summary>
    public RowType RowType { get; set; }

    /// <summary>
    /// 患者番号
    /// </summary>
    public string PtNum { get; set; }

    /// <summary>
    /// 氏名
    /// </summary>
    public string PtName { get; set; }

    /// <summary>
    /// カナ氏名
    /// </summary>
    public string KanaName { get; set; }

    /// <summary>
    /// 性別
    /// </summary>
    public string Sex { get; set; }

    /// <summary>
    /// 生年月日
    /// </summary>
    public string Birthday { get; set; }

    /// <summary>
    /// 生年月日(和暦)
    /// </summary>
    public string BirthdayW { get; set; }

    /// <summary>
    /// 生年月日(和暦西暦)
    /// </summary>
    public string BirthdayWS { get; set; }

    /// <summary>
    /// 年齢
    /// </summary>
    public string Age
    {
        get => (ageyear < 0) ? "" : ageyear.ToString();
    }

    /// <summary>
    /// 年齢(月)
    /// </summary>
    public string AgeMonth
    {
        get => (ageyear < 0 || agemonth < 0) ? "" : string.Format("{0}歳{1}ヶ月", ageyear, agemonth);
    }

    /// <summary>
    /// 死亡日
    /// </summary>
    public string DeathDate { get; set; }

    /// <summary>
    /// 郵便マーク
    /// </summary>
    public string HomePostMark { get; set; }

    /// <summary>
    /// 郵便番号
    /// </summary>
    public string HomePost { get; set; }

    /// <summary>
    /// 住所
    /// </summary>
    public string HomeAddress
    {
        get => HomeAddress1 + " " + HomeAddress2;
    }

    /// <summary>
    /// 住所１
    /// </summary>
    public string HomeAddress1 { get; set; }

    /// <summary>
    /// 住所２
    /// </summary>
    public string HomeAddress2 { get; set; }

    /// <summary>
    /// 電話
    /// </summary>
    public string Tel
    {
        get =>
            Tel1.AsString() != "" ? Tel1 :
            Tel2.AsString() != "" ? Tel2 :
            RenrakuTel;
    }

    /// <summary>
    /// 電話１
    /// </summary>
    public string Tel1 { get; set; }

    /// <summary>
    /// 電話２
    /// </summary>
    public string Tel2 { get; set; }

    /// <summary>
    /// メールアドレス
    /// </summary>
    public string Mail { get; set; }

    /// <summary>
    /// 世帯主
    /// </summary>
    public string Setainusi { get; set; }

    /// <summary>
    /// 世帯主との続柄
    /// </summary>
    public string Zokugara { get; set; }

    /// <summary>
    /// 職業
    /// </summary>
    public string Job { get; set; }

    /// <summary>
    /// 連絡先名称
    /// </summary>
    public string RenrakuName { get; set; }

    /// <summary>
    /// 連絡先郵便番号
    /// </summary>
    public string RenrakuPost { get; set; }

    /// <summary>
    /// 連絡先住所１
    /// </summary>
    public string RenrakuAddress1 { get; set; }

    /// <summary>
    /// 連絡先住所２
    /// </summary>
    public string RenrakuAddress2 { get; set; }

    /// <summary>
    /// 連絡先電話番号
    /// </summary>
    public string RenrakuTel { get; set; }

    /// <summary>
    /// 連絡先備考
    /// </summary>
    public string RenrakuMemo { get; set; }

    /// <summary>
    /// 勤務先名称
    /// </summary>
    public string OfficeName { get; set; }

    /// <summary>
    /// 勤務先郵便番号
    /// </summary>
    public string OfficePost { get; set; }

    /// <summary>
    /// 勤務先住所１
    /// </summary>
    public string OfficeAddress1 { get; set; }

    /// <summary>
    /// 勤務先住所２
    /// </summary>
    public string OfficeAddress2 { get; set; }

    /// <summary>
    /// 勤務先電話番号
    /// </summary>
    public string OfficeTel { get; set; }

    /// <summary>
    /// 勤務先備考
    /// </summary>
    public string OfficeMemo { get; set; }

    /// <summary>
    /// 領収証明細
    /// </summary>
    public string IsRyosyuDetail { get; set; }

    /// <summary>
    /// 主治医
    /// </summary>
    public string PrimaryDoctor { get; set; }

    /// <summary>
    /// テスト患者区分
    /// </summary>
    public string IsTester { get; set; }

    /// <summary>
    /// 患者登録日時
    /// </summary>
    public string CreateDate { get; set; }

    /// <summary>
    /// 初回来院日
    /// </summary>
    public string FirstVisitDate { get; set; }

    /// <summary>
    /// 最終来院日
    /// </summary>
    public string LastVisitDate { get; set; }

    /// <summary>
    /// 患者コメント
    /// </summary>
    public string PtCmt { get; set; }

    /// <summary>
    /// 調整額
    /// </summary>
    public string AdjFutan { get; set; }

    /// <summary>
    /// 調整率
    /// </summary>
    public string AdjRate { get; set; }

    /// <summary>
    /// 自動算定
    /// </summary>
    public string AutoSantei { get; set; }

    /// <summary>
    /// 患者グループ
    /// </summary>
    public List<CoPtInfModel.PtGrp> PtGrps { get; set; }

    /// <summary>
    /// 年齢基準日
    /// </summary>
    public int AgeBaseDate { get; set; }

    /// <summary>
    /// 年齢（年）
    /// </summary>
    private int ageyear
    {
        get => GetAge(1);
    }

    /// <summary>
    /// 年齢（月）
    /// </summary>
    private int agemonth
    {
        get => GetAge(2);
    }

    /// <summary>
    /// 年齢取得
    /// </summary>
    /// <param name="mode">1:年 2:月 3:日</param>
    /// <returns></returns>
    private int GetAge(int mode)
    {
        int aage = -1; int amonth = -1; int aday = -1;
        int birthday = (Birthday ?? "") == "" ? 0 : CIUtil.ShowSDateToSDate(Birthday ?? string.Empty);

        if (birthday > 0 && AgeBaseDate > 0)
        {
            CIUtil.SDateToDecodeAge(birthday, AgeBaseDate, ref aage, ref amonth, ref aday);
            switch (mode)
            {
                case 1: return aage;
                case 2: return amonth;
                case 3: return aday;
            }
        }
        return -1;
    }

    #region 処方・病名一覧
    /// <summary>
    /// 診療行為コード・病名コード
    /// </summary>
    public string ItemCd { get; set; }

    /// <summary>
    /// 漢字名称・病名
    /// </summary>
    public string ItemName { get; set; }

    /// <summary>
    /// 数量
    /// </summary>
    public string Suryo { get; set; }

    /// <summary>
    /// 単位名称
    /// </summary>
    public string UnitName { get; set; }

    /// <summary>
    /// 診療日・開始日
    /// </summary>
    public string SinDate { get; set; }

    /// <summary>
    /// 転帰日
    /// </summary>
    public string TenkiDate { get; set; }

    /// <summary>
    /// 転帰区分
    /// </summary>
    public string TenkiKbn { get; set; }
    #endregion

    #region CSV出力用
    public CoPtByomeiModel PtByomei { get; set; }

    public CoPtHokenModel PtHoken { get; set; }

    public CoRaiinInfModel RaiinInf { get; set; }

    public CoOdrInfModel OdrInf { get; set; }

    public CoSinKouiModel SinKoui { get; set; }

    public CoKarteInfModel KarteInf { get; set; }

    public CoKensaModel KensaInf { get; set; }

    #endregion
}
