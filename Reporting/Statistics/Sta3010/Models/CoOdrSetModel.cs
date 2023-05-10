namespace Reporting.Statistics.Sta3010.Models;

public class CoOdrSetModel
{

    /// <summary>
    /// セット区分コード
    /// </summary>
    public int SetKbn { get; set; }

    /// <summary>
    /// セット区分枝番
    /// </summary>
    public int SetKbnEdaNo { get; set; }

    /// <summary>
    /// セット区分枝番+1
    /// </summary>
    public int SetKbnEdaNoPlus1
    {
        get => SetKbnEdaNo + 1;
    }

    /// <summary>
    /// セット区分名称
    /// </summary>
    public string SetKbnName { get; set; } = string.Empty;

    /// <summary>
    /// 階層１
    /// </summary>
    public int Level1 { get; set; }

    /// <summary>
    /// 階層２
    /// </summary>
    public int Level2 { get; set; }

    /// <summary>
    /// 階層３
    /// </summary>
    public int Level3 { get; set; }

    /// <summary>
    /// セットコード
    /// </summary>
    public int SetCd { get; set; }

    /// <summary>
    /// セット名称
    /// </summary>
    public string SetName { get; set; } = string.Empty;

    /// <summary>
    /// 体重別セット
    /// </summary>
    public int WeightKbn { get; set; }

    /// <summary>
    /// RP番号
    /// </summary>
    public long RpNo { get; set; }

    /// <summary>
    /// RP番号枝番
    /// </summary>
    public long RpEdaNo { get; set; }

    /// <summary>
    /// 行為区分
    /// </summary>
    public int OdrKouiKbn { get; set; }

    /// <summary>
    /// 並び順
    /// </summary>
    public int SortNo { get; set; }

    /// <summary>
    /// 診療行為グループ
    /// </summary>
    public int GroupKoui { get; set; }

    /// <summary>
    /// 院内院外
    /// </summary>
    public int InoutKbn { get; set; }

    /// <summary>
    /// 至急区分
    /// </summary>
    public int SikyuKbn { get; set; }

    /// <summary>
    /// 処方種別
    /// </summary>
    public int SyohoSbt { get; set; }

    /// <summary>
    /// 算定区分
    /// </summary>
    public int SanteiKbn { get; set; }

    /// <summary>
    /// 透析区分
    /// </summary>
    public int TosekiKbn { get; set; }

    /// <summary>
    /// 行番号
    /// </summary>
    public int RowNo { get; set; }

    /// <summary>
    /// 診療行為コード
    /// </summary>
    public string ItemCd { get; set; } = string.Empty;

    /// <summary>
    /// 診療行為名称
    /// </summary>
    public string ItemName { get; set; } = string.Empty;

    /// <summary>
    /// 数量
    /// </summary>
    public double Suryo { get; set; }

    /// <summary>
    /// 単位
    /// </summary>
    public string UnitName { get; set; } = string.Empty;

    /// <summary>
    /// 薬剤区分
    /// </summary>
    public int DrugKbn { get; set; }

    /// <summary>
    /// 処方せん記載区分
    /// </summary>
    public int SyohoKbn { get; set; }

    /// <summary>
    /// 処方せん記載制限区分
    /// </summary>
    public int SyohoLimitKbn { get; set; }

    /// <summary>
    /// 検査項目コード
    /// </summary>
    public string KensaItemCd { get; set; } = string.Empty;

    /// <summary>
    /// 外注検査項目コード１
    /// </summary>
    public string CenterItemCd1 { get; set; } = string.Empty;

    /// <summary>
    /// 外注検査項目コード２
    /// </summary>
    public string CenterItemCd2 { get; set; } = string.Empty;

    /// <summary>
    /// 外注検査項目コード
    /// </summary>
    public string CenterItemCd
    {
        get => CenterItemCd1 + CenterItemCd2;
    }

    /// <summary>
    /// 有効期限
    /// </summary>
    public int MaxEndDate { get; set; }

    /// <summary>
    /// 有効期限
    /// </summary>
    public int EndDate
    {
        get => MaxEndDate <= 0 ? 99999999 : MaxEndDate;
    }

    /// <summary>
    /// 投薬
    /// </summary>
    public bool IsDrug
    {
        get => (OdrKouiKbn >= 20 && OdrKouiKbn <= 23) || OdrKouiKbn == 28 || OdrKouiKbn == 100 || OdrKouiKbn == 101;
    }

    /// <summary>
    /// 検査
    /// </summary>
    public bool IsKensa
    {
        get => (OdrKouiKbn >= 60 && OdrKouiKbn < 70);
    }

    /// <summary>
    /// 院内院外区分表示
    /// </summary>
    public bool IsShowInOut
    {
        get => (OdrKouiKbn.ToString().Length == 3) || (OdrKouiKbn >= 20 && OdrKouiKbn < 30) || (OdrKouiKbn >= 60 && OdrKouiKbn < 70);
    }

}
