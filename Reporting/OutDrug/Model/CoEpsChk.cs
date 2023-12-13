using Entity.Tenant;

namespace Reporting.OutDrug.Model;

public class CoEpsChk
{
    public EpsChk EpsChk { get; set; }
    public EpsChkDetail EpsChkDetail { get; set; }
    public CoEpsChk(EpsChk epsChk, EpsChkDetail epsChkDetail)
    {
        EpsChk = epsChk;
        EpsChkDetail = epsChkDetail;
    }

    /// <summary>
    /// チェック結果
    /// 0:重複等あり(医師許可なし)
    /// 1:重複等あり(医師許可)
    /// 2:重複等なし
    /// </summary>
    public int CheckResult
    {
        get => EpsChk.CheckResult;
    }

    /// <summary>
    /// 今回＿対象薬品コード
    /// </summary>
    public string TargetPharmaceuticalCode
    {
        get => EpsChkDetail.TargetPharmaceuticalCode ?? string.Empty;
    }

    /// <summary>
    /// 今回＿対象薬品コード種別
    /// </summary>
    public string TargetPharmaceuticalCodeType
    {
        get => EpsChkDetail.TargetPharmaceuticalCodeType ?? string.Empty;
    }

    /// <summary>
    /// 今回＿対象薬品名称
    /// </summary>
    public string TargetPharmaceuticalName
    {
        get => EpsChkDetail.TargetPharmaceuticalName ?? string.Empty;
    }

    /// <summary>
    /// 投与理由コメント
    /// </summary>
    public string Comment
    {
        get => EpsChkDetail.Comment ?? string.Empty;
    }

}
