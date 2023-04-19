using EmrCalculateApi.Receipt.ViewModels;

namespace Reporting.Accounting.Model;

public class CoAccountingListModel
{
    CoHpInfModel HpInfModel { get; } 
    public List<SinMeiViewModel> SinMeiViewModels { get; } 
    public List<CoKaikeiInfListModel> KaikeiInfListModels { get; }
    public CoAccountingListModel(CoHpInfModel hpInfModel, List<CoKaikeiInfListModel> kaikeiInfListModels)
    {
        HpInfModel = hpInfModel;
        KaikeiInfListModels = kaikeiInfListModels;
    }

    /// <summary>
    /// 医療機関名
    /// </summary>
    public string HpName
    {
        get => HpInfModel.HpName;
    }
    /// <summary>
    /// 医療機関住所
    /// </summary>
    public string HpAddress
    {
        get => HpInfModel.Address1 + HpInfModel.Address2;
    }
    public string HpAddress1
    {
        get => HpInfModel.Address1;
    }
    public string HpAddress2
    {
        get => HpInfModel.Address2;
    }
    /// <summary>
    /// 医療機関郵便番号
    /// </summary>
    public string HpPostCd
    {
        get => HpInfModel.PostCd;
    }
    public string HpPostCdDsp
    {
        get => HpInfModel.PostCdDsp;
    }
    /// <summary>
    /// 開設者氏名
    /// </summary>
    public string KaisetuName
    {
        get => HpInfModel.KaisetuName;
    }
    /// <summary>
    /// 医療機関電話番号
    /// </summary>
    public string HpTel
    {
        get => HpInfModel.Tel;
    }

    /// <summary>
    ///  自費種別の種類のリストを取得する
    /// </summary>
    /// <returns></returns>
    public List<int> GetJihiSbt()
    {
        HashSet<int> ret = new HashSet<int>();

        foreach (CoKaikeiInfListModel kaikeiInfListModel in KaikeiInfListModels)
        {
            foreach ((int santeiKbn, int jihiSbt, double kingaku) jihiKoumokuDtl in kaikeiInfListModel.JihiKoumokuDtls)
            {
                if (jihiKoumokuDtl.jihiSbt > 0)
                {
                    ret.Add(jihiKoumokuDtl.jihiSbt);
                }
            }
        }

        return ret.ToList();
    }

    public string GetGroupCd(int grpId)
    {
        HashSet<string> ret = new HashSet<string>();

        foreach (CoKaikeiInfListModel kaikeiInfListModel in KaikeiInfListModels)
        {
            foreach (CoPtGrpInfModel ptGrpInf in kaikeiInfListModel.PtGroupInfModels.FindAll(p => p.GroupId == grpId))
            {
                if (ptGrpInf.GroupCode != "")
                {
                    ret.Add(ptGrpInf.GroupCode);
                }
            }
        }

        if (ret.Count() == 1)
        {
            return ret.First();
        }
        else
        {
            return "";
        }
    }
}
