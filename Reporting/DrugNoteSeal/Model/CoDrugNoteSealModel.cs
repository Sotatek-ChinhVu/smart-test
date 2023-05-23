namespace Reporting.DrugNoteSeal.Model;

public class CoDrugNoteSealModel
{
    // 医療機関情報
    public CoHpInfModel HpInfModel { get; set; }

    // 患者情報
    public CoPtInfModel PtInfModel { get; set; }

    // 来院情報
    public CoRaiinInfModel RaiinInfModel { get; set; }

    // オーダー情報
    public List<CoOdrInfModel> OdrInfModels { get; set; }

    // オーダー情報詳細
    public List<CoOdrInfDetailModel> OdrInfDetailModels { get; set; }

    public CoDrugNoteSealModel(
        CoHpInfModel hpInf, CoPtInfModel ptInf, CoRaiinInfModel raiinInf,
        List<CoOdrInfModel> odrInfs, List<CoOdrInfDetailModel> odrDtls)
    {
        HpInfModel = hpInf;
        PtInfModel = ptInf;
        RaiinInfModel = raiinInf;
        OdrInfModels = odrInfs;
        OdrInfDetailModels = odrDtls;
    }
    
    public CoDrugNoteSealModel()
    {
        HpInfModel = new();
        PtInfModel = new();
        RaiinInfModel = new();
        OdrInfModels = new();
        OdrInfDetailModels = new();
    }

    /// <summary>
    /// 患者番号
    /// </summary>
    public long PtNum
    {
        get => PtInfModel.PtNum;
    }
    /// <summary>
    /// 患者ID
    /// </summary>
    public long PtId
    {
        get => PtInfModel.PtId;
    }
    /// <summary>
    /// 患者氏名
    /// </summary>
    public string PtName
    {
        get => PtInfModel.Name ?? string.Empty;
    }
    /// <summary>
    /// 患者カナ氏名
    /// </summary>
    public string PtKanaName
    {
        get => PtInfModel.KanaName ?? string.Empty;
    }
    /// <summary>
    /// 性別
    /// </summary>
    public int Sex
    {
        get => PtInfModel.Sex;
    }

    /// <summary>
    /// 性別を"男", "女"で返す
    /// </summary>
    public string PtSex
    {
        get
        {
            string ret = "男";

            if (Sex == 2)
            {
                ret = "女";
            }

            return ret;
        }
    }
    /// <summary>
    /// 生年月日
    /// </summary>
    public int BirthDay
    {
        get => PtInfModel.Birthday;
    }
    /// <summary>
    /// 年齢
    /// </summary>
    public int Age
    {
        get => PtInfModel.Age;
    }
    /// <summary>
    /// 診療科名
    /// </summary>
    public string KaName
    {
        get => RaiinInfModel != null ? RaiinInfModel.KaName : string.Empty;
    }
    /// <summary>
    /// 担当医氏名
    /// </summary>
    public string TantoName
    {
        get => RaiinInfModel != null ? RaiinInfModel.TantoName : string.Empty;
    }
    /// <summary>
    /// 受付番号
    /// </summary>
    public int UketukeNo
    {
        get => RaiinInfModel != null ? RaiinInfModel.UketukeNo : 0;
    }
    /// <summary>
    /// 医療機関名
    /// </summary>
    public string HpName
    {
        get => HpInfModel != null ? HpInfModel.HpName : string.Empty;
    }
    /// <summary>
    /// 医療機関電話番号
    /// </summary>
    public string HpTel
    {
        get => HpInfModel != null ? HpInfModel.Tel : string.Empty;
    }
    /// <summary>
    /// 医療機関所在地
    /// </summary>
    public string HpAddress
    {
        get => HpInfModel != null ? HpInfModel.Address : string.Empty;
    }
    /// <summary>
    /// 医療機関所在地１
    /// </summary>
    public string HpAddress1
    {
        get => HpInfModel != null ? HpInfModel.Address1 : string.Empty;
    }
    /// <summary>
    /// 医療機関所在地２
    /// </summary>
    public string HpAddress2
    {
        get => HpInfModel != null ? HpInfModel.Address2 : string.Empty;
    }
    /// <summary>
    /// 医療機関FAX番号
    /// </summary>
    public string HpFaxNo
    {
        get => HpInfModel != null ? HpInfModel.FaxNo : string.Empty;
    }
    /// <summary>
    /// 医療機関その他連絡先
    /// </summary>
    public string HpOtherContacts
    {
        get => HpInfModel != null ? HpInfModel.OtherContacts : string.Empty;
    }
}