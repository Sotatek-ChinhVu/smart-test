using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Enum;
using Reporting.CommonMasters.Common.Interface;

namespace Reporting.OrderLabel.Model;

public class RsvkrtOdrInfModel : IOdrInfModel<RsvkrtOdrInfDetailModel>
{
    public RsvkrtOdrInf RsvkrtOdrInf { get; }

    private readonly IUserConfReportCommon _userConfReportCommon;
    private readonly IUserMstCache _userMstCache;

    public RsvkrtOdrInfModel(IUserConfReportCommon userConfReportCommon, IUserMstCache userMstCache, RsvkrtOdrInf rsvkrtOdrInf, List<RsvkrtOdrInfDetailModel> rsvkrtOdrInfDetails)
    {
        _userConfReportCommon = userConfReportCommon;
        _userMstCache = userMstCache;
        RsvkrtOdrInf = rsvkrtOdrInf;
        OdrInfDetailModels = rsvkrtOdrInfDetails;
        OdrInfDetailModelView = new();
        SelectedOrderDetailModel = new();
    }

    public int Index { get; set; }

    /// <summary>
    /// ID
    /// </summary>
    public long Id
    {
        get { return RsvkrtOdrInf.Id; }
        set
        {
            if (RsvkrtOdrInf.Id == value) return;
            RsvkrtOdrInf.Id = value;
        }
    }

    /// <summary>
    /// 医療機関識別ID
    /// 
    /// </summary>
    public int HpId
    {
        get { return RsvkrtOdrInf.HpId; }
        set
        {
            if (RsvkrtOdrInf.HpId == value) return;
            RsvkrtOdrInf.HpId = value;
        }
    }

    /// <summary>
    /// 患者ID
    /// 患者を識別するためのシステム固有の番号
    /// </summary>
    public long PtId
    {
        get { return RsvkrtOdrInf.PtId; }
        set
        {
            if (RsvkrtOdrInf.PtId == value) return;
            RsvkrtOdrInf.PtId = value;
        }
    }

    /// <summary>
    /// 予約日
    /// yyyymmdd
    /// </summary>
    public int RsvDate
    {
        get { return RsvkrtOdrInf.RsvDate; }
        set
        {
            if (RsvkrtOdrInf.RsvDate == value) return;
            RsvkrtOdrInf.RsvDate = value;
        }
    }

    /// <summary>
    /// 予約カルテ番号
    /// 
    /// </summary>
    public long RsvkrtNo
    {
        get { return RsvkrtOdrInf.RsvkrtNo; }
        set
        {
            if (RsvkrtOdrInf.RsvkrtNo == value) return;
            RsvkrtOdrInf.RsvkrtNo = value;
        }
    }

    /// <summary>
    /// 剤番号
    /// 
    /// </summary>
    public long RpNo
    {
        get { return RsvkrtOdrInf.RpNo; }
        set
        {
            if (RsvkrtOdrInf.RpNo == value) return;
            RsvkrtOdrInf.RpNo = value;
        }
    }

    /// <summary>
    /// 剤枝番
    /// 剤に変更があった場合、カウントアップ
    /// </summary>
    public long RpEdaNo
    {
        get { return RsvkrtOdrInf.RpEdaNo; }
        set
        {
            if (RsvkrtOdrInf.RpEdaNo == value) return;
            RsvkrtOdrInf.RpEdaNo = value;
        }
    }

    /// <summary>
    /// オーダー行為区分
    /// 
    /// </summary>
    public int OdrKouiKbn
    {
        get { return RsvkrtOdrInf.OdrKouiKbn; }
        set
        {
            if (RsvkrtOdrInf.OdrKouiKbn == value) return;
            RsvkrtOdrInf.OdrKouiKbn = value;
        }
    }

    public int GroupOdrKouiKbn
    {
        get => CIUtil.GetGroupKoui(RsvkrtOdrInf.OdrKouiKbn);
    }

    /// <summary>
    /// 剤名称
    /// 
    /// </summary>
    public string RpName
    {
        get { return RsvkrtOdrInf.RpName ?? string.Empty; }
        set
        {
            if (RsvkrtOdrInf.RpName == value) return;
            RsvkrtOdrInf.RpName = value;
        }
    }

    /// <summary>
    /// 院内院外区分
    /// "0: 院内
    /// 1: 院外"
    /// </summary>
    public int InoutKbn
    {
        get { return RsvkrtOdrInf.InoutKbn; }
        set
        {
            if (RsvkrtOdrInf.InoutKbn == value) return;
            RsvkrtOdrInf.InoutKbn = value;
        }
    }

    /// <summary>
    /// 至急区分
    /// "0:通常 
    /// 1:至急"
    /// </summary>
    public int SikyuKbn
    {
        get { return RsvkrtOdrInf.SikyuKbn; }
        set
        {
            if (RsvkrtOdrInf.SikyuKbn == value) return;
            RsvkrtOdrInf.SikyuKbn = value;
        }
    }

    /// <summary>
    /// 処方種別
    /// "0: 日数判断
    /// 1: 臨時
    /// 2: 常態"
    /// </summary>
    public int SyohoSbt
    {
        get { return RsvkrtOdrInf.SyohoSbt; }
        set
        {
            if (RsvkrtOdrInf.SyohoSbt == value) return;
            RsvkrtOdrInf.SyohoSbt = value;
        }
    }

    /// <summary>
    /// 算定区分
    /// "1: 算定外
    /// 2: 自費算定"
    /// </summary>
    public int SanteiKbn
    {
        get { return RsvkrtOdrInf.SanteiKbn; }
        set
        {
            if (RsvkrtOdrInf.SanteiKbn == value) return;
            RsvkrtOdrInf.SanteiKbn = value;
        }
    }

    /// <summary>
    /// 透析区分
    /// "0: 透析以外
    /// 1: 透析前
    /// 2: 透析後"
    /// </summary>
    public int TosekiKbn
    {
        get { return RsvkrtOdrInf.TosekiKbn; }
        set
        {
            if (RsvkrtOdrInf.TosekiKbn == value) return;
            RsvkrtOdrInf.TosekiKbn = value;
        }
    }

    /// <summary>
    /// 日数回数
    /// 処方日数
    /// </summary>
    public int DaysCnt
    {
        get { return RsvkrtOdrInf.DaysCnt; }
        set
        {
            if (RsvkrtOdrInf.DaysCnt == value) return;
            RsvkrtOdrInf.DaysCnt = value;
        }
    }

    /// <summary>
    /// 削除区分
    /// "1: 削除
    /// 2: 実施"
    /// </summary>
    public int IsDeleted
    {
        get { return RsvkrtOdrInf.IsDeleted; }
        set
        {
            if (RsvkrtOdrInf.IsDeleted == value) return;
            RsvkrtOdrInf.IsDeleted = value;
            IsDeleting = true;
        }
    }

    /// <summary>
    /// 並び順
    /// 
    /// </summary>
    public int SortNo
    {
        get { return RsvkrtOdrInf.SortNo; }
        set
        {
            if (RsvkrtOdrInf.SortNo == value) return;
            RsvkrtOdrInf.SortNo = value;
        }
    }

    /// <summary>
    /// 作成日時
    /// 
    /// </summary>
    public DateTime CreateDate
    {
        get { return RsvkrtOdrInf.CreateDate; }
        set
        {
            if (RsvkrtOdrInf.CreateDate == value) return;
            RsvkrtOdrInf.CreateDate = value;
        }
    }

    /// <summary>
    /// 作成者
    /// 
    /// </summary>
    public int CreateId
    {
        get { return RsvkrtOdrInf.CreateId; }
        set
        {
            if (RsvkrtOdrInf.CreateId == value) return;
            RsvkrtOdrInf.CreateId = value;
        }
    }

    /// <summary>
    /// 作成端末
    /// 
    /// </summary>
    public string CreateMachine
    {
        get { return RsvkrtOdrInf.CreateMachine ?? string.Empty; }
        set
        {
            if (RsvkrtOdrInf.CreateMachine == value) return;
            RsvkrtOdrInf.CreateMachine = value;
        }
    }

    /// <summary>
    /// 更新日時
    /// 
    /// </summary>
    public DateTime UpdateDate
    {
        get { return RsvkrtOdrInf.UpdateDate; }
        set
        {
            if (RsvkrtOdrInf.UpdateDate == value) return;
            RsvkrtOdrInf.UpdateDate = value;
        }
    }

    /// <summary>
    /// 更新者
    /// 
    /// </summary>
    public int UpdateId
    {
        get { return RsvkrtOdrInf.UpdateId; }
        set
        {
            if (RsvkrtOdrInf.UpdateId == value) return;
            RsvkrtOdrInf.UpdateId = value;
        }
    }

    /// <summary>
    /// 更新端末
    /// 
    /// </summary>
    public string UpdateMachine
    {
        get { return RsvkrtOdrInf.UpdateMachine ?? string.Empty; }
        set
        {
            if (RsvkrtOdrInf.UpdateMachine == value) return;
            RsvkrtOdrInf.UpdateMachine = value;
        }
    }

    #region Exposed properties
    private List<RsvkrtOdrInfDetailModel> rsvkrtOdrInfDetailModels = new();

    public List<RsvkrtOdrInfDetailModel> OdrInfDetailModels
    {
        get => rsvkrtOdrInfDetailModels;
        set
        {
            rsvkrtOdrInfDetailModels = value;
            if (value != null)
            {
                foreach (var odrDetail in OdrInfDetailModels)
                {
                    if (odrDetail.ItemCd == ItemCdConst.Con_TouyakuOrSiBunkatu)
                    {
                        var usage = OdrInfDetailModels.FirstOrDefault(d => d.IsStandardUsage);
                        if (usage != null)
                        {
                            odrDetail.BunkatuKoui = usage.SinKouiKbn;
                        }
                    }
                }
            }
        }
    }

    public List<RsvkrtOdrInfDetailModel> OdrInfDetailModelView { get; set; }

    public List<RsvkrtOdrInfDetailModel> OdrInfDetailModelsIgnoreEmpty
    {
        get
        {
            if (OdrInfDetailModels == null)
            {
                return OdrInfDetailModels ?? new();
            }
            return new List<RsvkrtOdrInfDetailModel>(OdrInfDetailModels?.Where(o => !o.IsEmpty)?.ToList() ?? new());
        }
    }

    public RsvkrtOdrInfDetailModel SelectedOrderDetailModel { get; set; }

    public bool IsShowTitle
    {
        get => true;
    }

    public string OdrInfTitle
    {
        get => BuildOdrTitle();
    }

    private string BuildOdrTitle()
    {
        string result = Index + ") ";

        int displaySetName = _userConfReportCommon.DisplaySetName(HpId);
        int displayUserInput = _userConfReportCommon.DisplayUserInput(HpId);
        int displayTimeInput = _userConfReportCommon.DisplayTimeInput(HpId);

        if (displaySetName == 1)
        {
            if (!string.IsNullOrEmpty(RpName))
            {
                result += " 【" + RpName + "】";
            }
            else
            {
                if (OdrInfDetailModels.Count > 0)
                {
                    // 項目コード4桁以下は無視する
                    var odrInfDetail = OdrInfDetailModels.FirstOrDefault(detail => detail.ItemCd?.Length > 4);
                    if (odrInfDetail != null)
                    {
                        result += odrInfDetail.ItemName + "...";
                    }
                    else
                    {
                        // コメント/部位しかないRpの場合は先頭を
                        result += OdrInfDetailModels[0].ItemName + "...";
                    }
                }
            }
        }

        if (displayTimeInput == 1)
        {
            // 入力日時
            if (CreateDate != DateTime.MinValue)
            {
                result += " " + CIUtil.GetCIDateTimeStr(CreateDate);
            }
        }

        if (displayUserInput == 1)
        {
            //入力者
            if (CreateId != 0)
            {
                result += " " + _userMstCache.GetUserSNameIncludedDeleted(HpId ,CreateId);
            }
        }

        return result;
    }

    public string DrugPrice
    {
        get
        {
            string result = string.Empty;
            if (_userConfReportCommon.DisplayDrugPrice(HpId) == 0) return result;

            if (IsDrug)
            {
                double price = 0;
                foreach (var detail in OdrInfDetailModels)
                {
                    if (detail.TermVal != 0)
                    {
                        if (detail.SyohoKbn == 3)
                        {
                            price += detail.Suryo * detail.TermVal * detail.Yakka;
                        }
                        else
                        {
                            price += detail.Suryo * detail.TermVal * detail.Ten;
                        }
                    }
                }
                if (price > 0 && !double.IsNaN(price))
                {
                    result += price + "円";
                }
            }
            return result;
        }
    }

    // 処方 - Drug
    public bool IsDrug
    {
        get
        {
            return OdrKouiKbn >= 20 && OdrKouiKbn <= 23;
        }
    }

    // 注射 - Injection
    public bool IsInjection
    {
        get
        {
            return OdrKouiKbn >= 30 && OdrKouiKbn <= 34;
        }
    }

    public bool IsExpanded { get; set; }

    public bool IsShowCollapsedTitle
    {
        get => !IsExpanded;
    }

    public bool IsShownCheckbox { get; set; }

    public bool IsChecked
    {
        get => !OdrInfDetailModels.Any(p => !p.IsChecked);
        set
        {
            OdrInfDetailModels.ToList().ForEach(p => p.IsChecked = value);
        }
    }

    public bool IsSelected { get; set; }

    public string ID { get; } = Guid.NewGuid().ToString();

    public UsingDrugType UsingType
    {
        get
        {
            if (IsDrug || IsInjection)
            {
                foreach (RsvkrtOdrInfDetailModel odrInfDetailModel in this.OdrInfDetailModels)
                {
                    if (odrInfDetailModel.SinKouiKbn == ReportOdrKouiKbnConst.Naifuku)
                    {
                        return UsingDrugType.Drinking;
                    }
                    else if (odrInfDetailModel.SinKouiKbn == ReportOdrKouiKbnConst.Tonpuku)
                    {
                        return UsingDrugType.AsOrder;
                    }
                    else if (odrInfDetailModel.SinKouiKbn == ReportOdrKouiKbnConst.Gaiyo)
                    {
                        return UsingDrugType.OnSkin;
                    }
                }
            }
            return UsingDrugType.None;
        }
    }

    public bool IsShownReleasedDrugColumn { get; set; }

    public bool IsShownQuantityColumn { get; set; }

    public bool IsShownUnitColumn { get; set; }

    public double ItemNameColumnWidth { get; set; }

    public double QuantityColumnWidth { get; set; }

    public double UnitColumnWidth { get; set; }

    public double ReleasedDrugColumnWidth { get; set; }

    public bool IsItemNameReadOnly => false;

    public bool IsQuantityReadOnly => false;

    public bool IsShowKensaGaichu => false;

    public string GUID { get; } = Guid.NewGuid().ToString();

    public bool IsVisible
    {
        get => IsDeleted == 0;
    }

    public bool IsAddNew
    {
        get => Id == 0;
    }


    public bool IsDeleting = false;

    public bool IsSameGroupWith(RsvkrtOdrInfModel other)
    {
        return this.GroupOdrKouiKbn == other.GroupOdrKouiKbn
            && this.InoutKbn == other.InoutKbn
            && this.SikyuKbn == other.SikyuKbn
            && this.TosekiKbn == other.TosekiKbn
            && this.SyohoSbt == other.SyohoSbt
            && this.SanteiKbn == other.SanteiKbn;
    }

    public RsvkrtOdrInfModel DeepClone()
    {
        RsvkrtOdrInf newOdrInf = new RsvkrtOdrInf()
        {
            HpId = this.HpId,
            PtId = this.PtId,
            RsvkrtNo = this.RsvkrtNo,
            RsvDate = this.RsvDate,
            RpNo = this.RpNo,
            RpEdaNo = this.RpEdaNo,
            OdrKouiKbn = this.OdrKouiKbn,
            RpName = this.RpName,
            InoutKbn = this.InoutKbn,
            SikyuKbn = this.SikyuKbn,
            SyohoSbt = this.SyohoSbt,
            SanteiKbn = this.SanteiKbn,
            TosekiKbn = this.TosekiKbn,
            DaysCnt = this.DaysCnt,
            IsDeleted = this.IsDeleted,
            SortNo = this.SortNo
        };

        List<RsvkrtOdrInfDetailModel> newOdrDetails = new List<RsvkrtOdrInfDetailModel>();
        foreach (var odrDetail in this.OdrInfDetailModels)
        {
            RsvkrtOdrInfDetail newOdrDetail = new RsvkrtOdrInfDetail()
            {
                HpId = odrDetail.HpId,
                PtId = odrDetail.PtId,
                RsvkrtNo = odrDetail.RsvkrtNo,
                RsvDate = odrDetail.RsvDate,
                RpNo = odrDetail.RpNo,
                RpEdaNo = odrDetail.RpEdaNo,
                RowNo = odrDetail.RowNo,
                SinKouiKbn = odrDetail.SinKouiKbn,
                ItemCd = odrDetail.ItemCd,
                ItemName = odrDetail.ItemName,
                Suryo = odrDetail.Suryo,
                UnitName = odrDetail.UnitName,
                UnitSbt = odrDetail.UnitSBT,
                TermVal = odrDetail.TermVal,
                KohatuKbn = odrDetail.KohatuKbn,
                SyohoKbn = odrDetail.SyohoKbn,
                SyohoLimitKbn = odrDetail.SyohoLimitKbn,
                DrugKbn = odrDetail.DrugKbn,
                YohoKbn = odrDetail.YohoKbn,
                Kokuji1 = odrDetail.Kokuji1,
                Kokuji2 = odrDetail.Kokuji2,
                IsNodspRece = odrDetail.IsNodspRece,
                IpnCd = odrDetail.IpnCd,
                IpnName = odrDetail.IpnName,
                Bunkatu = odrDetail.Bunkatu,
                CmtName = odrDetail.CmtName,
                CmtOpt = odrDetail.CmtOpt,
                FontColor = odrDetail.FontColor
            };
            newOdrDetails.Add(new RsvkrtOdrInfDetailModel(newOdrDetail)
            {
                KensaMstModel = odrDetail.KensaMstModel,
                IpnMinYakkaMstModel = odrDetail.IpnMinYakkaMstModel,
                Ten = odrDetail.Ten,
                CmtCol1 = odrDetail.CmtCol1,
                CmtCol2 = odrDetail.CmtCol2,
                CmtCol3 = odrDetail.CmtCol3,
                CmtCol4 = odrDetail.CmtCol4,
                CmtColKeta1 = odrDetail.CmtColKeta1,
                CmtColKeta2 = odrDetail.CmtColKeta2,
                CmtColKeta3 = odrDetail.CmtColKeta3,
                CmtColKeta4 = odrDetail.CmtColKeta4,
            });
        }

        RsvkrtOdrInfModel result = new RsvkrtOdrInfModel(_userConfReportCommon, _userMstCache, newOdrInf, newOdrDetails);
        return result;
    }
    #endregion
}
