using Entity.Tenant;

namespace Reporting.OutDrug.Model;

public class CoYohoHosoku
{
    public TenMst TenMst { get; set; }
    public YohoHosoku YohoHosoku { get; set; }

    public CoYohoHosoku(YohoHosoku yohoHosoku, TenMst tenMst)
    {
        YohoHosoku = yohoHosoku;
        TenMst = tenMst;
    }

    /// <summary>
    /// 補足
    /// </summary>
    public string Hosoku
    {
        get { return TenMst == null ? YohoHosoku.Hosoku ?? string.Empty : TenMst.Name ?? string.Empty; }
    }

    /// <summary>
    /// 用法補足区分
    /// </summary>
    public int YohoHosokuKbn
    {
        get => YohoHosoku.YohoHosokuKbn;
    }

    /// <summary>
    /// 並び順
    /// </summary>
    public int SortNo
    {
        get => YohoHosoku.SortNo;
    }

}
