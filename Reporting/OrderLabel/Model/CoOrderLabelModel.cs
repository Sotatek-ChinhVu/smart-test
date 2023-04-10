namespace Reporting.OrderLabel.Model;

public class CoOrderLabelModel
{
    // 患者情報
    public CoPtInfModel PtInfModel { get; set; }

    // 来院情報
    public CoRaiinInfModel RaiinInfModel { get; set; }

    // オーダー情報
    public List<CoCommonOdrInfModel> OdrInfModels { get; set; }

    // オーダー情報詳細
    public List<CoCommonOdrInfDetailModel> OdrInfDetailModels { get; set; }

    public List<CoYoyakuModel> YoyakuModels { get; set; }
    public CoOrderLabelModel
        (CoPtInfModel ptInf, CoRaiinInfModel raiinInf,
            List<CoCommonOdrInfModel> odrInf, List<CoCommonOdrInfDetailModel> odrDtl,
            List<CoYoyakuModel> yoyakuModels, bool isYoyaku)
    {
        PtInfModel = ptInf;
        RaiinInfModel = raiinInf;
        OdrInfModels = odrInf;
        OdrInfDetailModels = odrDtl;
        YoyakuModels = yoyakuModels;
        IsYoyaku = isYoyaku;
    }

    public CoOrderLabelModel()
    {
        PtInfModel = new();
        RaiinInfModel = new();
        OdrInfModels = new();
        OdrInfDetailModels = new();
        YoyakuModels = new();
        IsYoyaku = new();
    }

    /// <summary>
    /// 患者番号
    /// </summary>
    public long PtNum
    {
        get => PtInfModel.PtNum;
    }
    /// <summary>
    /// 患者氏名
    /// </summary>
    public string PtName
    {
        get => PtInfModel.Name ?? "";
    }
    /// <summary>
    /// 診療科名
    /// </summary>
    public string KaName
    {
        get => RaiinInfModel != null ? RaiinInfModel.KaName : "";
    }
    /// <summary>
    /// 担当医氏名
    /// </summary>
    public string TantoName
    {
        get => RaiinInfModel != null ? RaiinInfModel.TantoName : "";
    }

    /// <summary>
    /// 予約オーダーかどうか
    /// </summary>
    public bool IsYoyaku { get; set; }
}
