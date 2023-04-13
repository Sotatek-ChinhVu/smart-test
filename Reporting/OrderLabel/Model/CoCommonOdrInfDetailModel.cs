namespace Reporting.OrderLabel.Model;

public class CoCommonOdrInfDetailModel
{
    public CoCommonOdrInfDetailModel(
           int hpId, long ptId, int sinDate, long raiinNo,
           long rpNo, long rpEdaNo, int rowNo,
           int odrKouiKbn,
           int sinKouiKbn,
           string itemCd, string itemName,
           double suryo, string unitName,
           int syohoKbn, int syohoLimitKbn,
           string bunkatu)
    {
        HpId = hpId;
        PtId = ptId;
        SinDate = sinDate;
        RaiinNo = raiinNo;
        RpNo = rpNo;
        RpEdaNo = rpEdaNo;
        RowNo = rowNo;
        OdrKouiKbn = odrKouiKbn;
        SinKouiKbn = sinKouiKbn;
        ItemCd = itemCd;
        ItemName = itemName;
        Suryo = suryo;
        UnitName = unitName;
        SyohoKbn = syohoKbn;
        SyohoLimitKbn = syohoLimitKbn;
        Bunkatu = bunkatu;

    }
    
    public CoCommonOdrInfDetailModel()
    {
        ItemCd = string.Empty;
        ItemName = string.Empty;
        UnitName = string.Empty;
        Bunkatu = string.Empty;

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
    /// オーダー行為区分
    /// </summary>
    public int OdrKouiKbn { get; set; }
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
    /// 処方せん記載区分
    ///             0: 指示なし（後発品のない先発品）
    ///             1: 変更不可
    ///             2: 後発品（他銘柄）への変更可 
    ///             3: 一般名処方
    /// </summary>
    public int SyohoKbn { get; set; }
    /// <summary>
    /// 処方せん記載制限区分
    ///             0: 制限なし
    ///             1: 剤形不可
    ///             2: 含量規格不可
    ///             3: 含量規格・剤形不可
    /// </summary>
    public int SyohoLimitKbn { get; set; }
}