using Entity.Tenant;

namespace Reporting.OutDrug.Model;

public class CoYohoMstModel
{
    public YohoMst YohoMst { get; set; }

    public CoYohoMstModel(YohoMst yohoMst)
    {
        YohoMst = yohoMst;
    }

    /// <summary>
    /// 用法コード
    /// </summary>
    public string YohoCd => YohoMst.YohoCd ?? string.Empty;

    /// <summary>
    /// 用法名称
    /// </summary>
    public string YohoName => YohoMst.YohoName ?? string.Empty;

    /// <summary>
    /// 使用開始日
    /// </summary>
    public int StartDate => YohoMst.StartDate;

    /// <summary>
    /// 使用終了日
    /// </summary>
    public int EndDate => YohoMst.EndDate;

}
