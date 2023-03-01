using Helper.Constants;
using Entity.Tenant;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace EmrCalculateApi.Ika.Models
{
    public class TodayOdrInfModel
    { 
        //public OdrInf OdrInf { get; } = null;

        //public TodayOdrInfModel(OdrInf odrInf, List<OdrInfDetail> odrInfDetails)
        //{
        //    OdrInf = odrInf;
        //    OdrInfDetailModels = new ManagedItemCollection<TodayOdrInfDetailModel>(odrInfDetails.Select(detail => new TodayOdrInfDetailModel(detail)));
        //}

        //public long Id
        //{
        //    get => OdrInf.Id;
        //}

        ///// <summary>
        ///// オーダー情報
        ///// </summary>
        ///// <summary>
        ///// 医療機関識別ID
        ///// </summary>
        //public int HpId
        //{
        //    get { return OdrInf.HpId; }
        //    set
        //    {
        //        if (OdrInf.HpId == value) return;
        //        OdrInf.HpId = value;
        //        //RaisePropertyChanged(() => HpId);
        //    }
        //}

        ///// <summary>
        ///// 患者ID
        /////     患者を識別するためのシステム固有の番号
        ///// </summary>
        //public long PtId
        //{
        //    get { return OdrInf.PtId; }
        //    set
        //    {
        //        if (OdrInf.PtId == value) return;
        //        OdrInf.PtId = value;
        //        //RaisePropertyChanged(() => PtId);
        //    }
        //}

        ///// <summary>
        ///// 診療日
        /////     yyyymmdd
        ///// </summary>
        //public int SinDate
        //{
        //    get { return OdrInf.SinDate; }
        //    set
        //    {
        //        if (OdrInf.SinDate == value) return;
        //        OdrInf.SinDate = value;
        //        //RaisePropertyChanged(() => SinDate);
        //    }
        //}

        ///// <summary>
        ///// 来院番号
        ///// </summary>
        //public long RaiinNo
        //{
        //    get { return OdrInf.RaiinNo; }
        //    set
        //    {
        //        if (OdrInf.RaiinNo == value) return;
        //        OdrInf.RaiinNo = value;
        //        //RaisePropertyChanged(() => RaiinNo);
        //    }
        //}

        ///// <summary>
        ///// 剤番号
        ///// </summary>
        //public long RpNo
        //{
        //    get { return OdrInf.RpNo; }
        //    set
        //    {
        //        if (OdrInf.RpNo == value) return;
        //        OdrInf.RpNo = value;
        //        //RaisePropertyChanged(() => RpNo);
        //    }
        //}

        ///// <summary>
        ///// 剤枝番
        /////     剤に変更があった場合、カウントアップ
        ///// </summary>
        //public long RpEdaNo
        //{
        //    get { return OdrInf.RpEdaNo; }
        //    set
        //    {
        //        if (OdrInf.RpEdaNo == value) return;
        //        OdrInf.RpEdaNo = value;
        //        //RaisePropertyChanged(() => RpEdaNo);
        //    }
        //}

        ///// <summary>
        ///// 保険組合せID
        ///// </summary>
        //public int HokenPid
        //{
        //    get { return OdrInf.HokenPid; }
        //    set
        //    {
        //        if (OdrInf.HokenPid == value) return;
        //        OdrInf.HokenPid = value;
        //        //RaisePropertyChanged(() => HokenPid);
        //    }
        //}

        ///// <summary>
        ///// オーダー行為区分
        ///// </summary>
        //public int OdrKouiKbn
        //{
        //    get { return OdrInf.OdrKouiKbn; }
        //    set
        //    {
        //        if (OdrInf.OdrKouiKbn == value) return;
        //        OdrInf.OdrKouiKbn = value;
        //        //RaisePropertyChanged(() => OdrKouiKbn);
        //        //RaisePropertyChanged(() => UsingType);
        //        //RaisePropertyChanged(() => GroupOdrKouiKbn);
        //        if (OdrInfDetailModels != null)
        //        {
        //            foreach (var detail in OdrInfDetailModels)
        //            {
        //                detail.OdrInfOdrKouiKbn = value;
        //            }
        //        }
        //    }
        //}

        //public int GroupOdrKouiKbn
        //{
        //    get => OdrInf.GroupKoui;
        //}

        ///// <summary>
        ///// 剤名称
        ///// </summary>
        //public string RpName
        //{
        //    get { return OdrInf.RpName; }
        //    set
        //    {
        //        if (OdrInf.RpName == value) return;
        //        OdrInf.RpName = value;
        //        //RaisePropertyChanged(() => RpName);
        //        //RaisePropertyChanged(() => OdrInfTitle);
        //    }
        //}

        ///// <summary>
        ///// 院内院外区分
        /////     0: 院内
        /////     1: 院外
        ///// </summary>
        //public int InoutKbn
        //{
        //    get { return OdrInf.InoutKbn; }
        //    set
        //    {
        //        if (OdrInf.InoutKbn == value) return;
        //        OdrInf.InoutKbn = value;
        //        //RaisePropertyChanged(() => InoutKbn);
        //        if (OdrInfDetailModels != null)
        //        {
        //            foreach (var detail in OdrInfDetailModels)
        //            {
        //                detail.InOutKbn = value;
        //            }
        //        }
        //    }
        //}

        ///// <summary>
        ///// 至急区分
        /////     0: 通常 
        /////     1: 至急
        ///// </summary>
        //public int SikyuKbn
        //{
        //    get { return OdrInf.SikyuKbn; }
        //    set
        //    {
        //        if (OdrInf.SikyuKbn == value) return;
        //        OdrInf.SikyuKbn = value;
        //        //RaisePropertyChanged(() => SikyuKbn);
        //    }
        //}

        ///// <summary>
        ///// 処方種別
        /////     0: 日数判断
        /////     1: 臨時
        /////     2: 常態
        ///// </summary>
        //public int SyohoSbt
        //{
        //    get { return OdrInf.SyohoSbt; }
        //    set
        //    {
        //        if (OdrInf.SyohoSbt == value) return;
        //        OdrInf.SyohoSbt = value;
        //        //RaisePropertyChanged(() => SyohoSbt);
        //    }
        //}

        ///// <summary>
        ///// 算定区分
        /////     1: 算定外
        /////     2: 自費算定
        ///// </summary>
        //public int SanteiKbn
        //{
        //    get { return OdrInf.SanteiKbn; }
        //    set
        //    {
        //        if (OdrInf.SanteiKbn == value) return;
        //        OdrInf.SanteiKbn = value;
        //        //RaisePropertyChanged(() => SanteiKbn);
        //    }
        //}

        ///// <summary>
        ///// 透析区分
        /////     0: 透析以外
        /////     1: 透析前
        /////     2: 透析後
        ///// </summary>
        //public int TosekiKbn
        //{
        //    get { return OdrInf.TosekiKbn; }
        //    set
        //    {
        //        if (OdrInf.TosekiKbn == value) return;
        //        OdrInf.TosekiKbn = value;
        //        //RaisePropertyChanged(() => TosekiKbn);
        //    }
        //}

        ///// <summary>
        ///// 日数回数
        /////     処方日数
        ///// </summary>
        //public int DaysCnt
        //{
        //    get { return OdrInf.DaysCnt; }
        //    set
        //    {
        //        if (OdrInf.DaysCnt == value) return;
        //        OdrInf.DaysCnt = value;
        //        //RaisePropertyChanged(() => DaysCnt);
        //    }
        //}

        ///// <summary>
        ///// 並び順
        ///// </summary>
        //public int SortNo
        //{
        //    get { return OdrInf.SortNo; }
        //    set
        //    {
        //        if (OdrInf.SortNo == value) return;
        //        OdrInf.SortNo = value;
        //        //RaisePropertyChanged(() => SortNo);
        //    }
        //}

        ///// <summary>
        ///// 削除区分
        /////     1:削除
        /////     2:未確定削除
        ///// </summary>
        //public int IsDeleted
        //{
        //    get { return OdrInf.IsDeleted; }
        //    set
        //    {
        //        if (OdrInf.IsDeleted == value) return;
        //        OdrInf.IsDeleted = value;
        //        //RaisePropertyChanged(() => IsDeleted);
        //        //RaisePropertyChanged(() => IsVisible);
        //    }
        //}

        ///// <summary>
        ///// 作成日時 
        ///// </summary>
        //public DateTime CreateDate
        //{
        //    get { return OdrInf.CreateDate; }
        //    set
        //    {
        //        if (OdrInf.CreateDate == value) return;
        //        OdrInf.CreateDate = value;
        //        //RaisePropertyChanged(() => CreateDate);
        //        //RaisePropertyChanged(() => OdrInfTitle);
        //    }
        //}

        ///// <summary>
        ///// 作成者
        ///// </summary>
        //public int CreateId
        //{
        //    get { return OdrInf.CreateId; }
        //    set
        //    {
        //        if (OdrInf.CreateId == value) return;
        //        OdrInf.CreateId = value;
        //        //RaisePropertyChanged(() => CreateId);
        //        //RaisePropertyChanged(() => OdrInfTitle);
        //    }
        //}

        ///// <summary>
        ///// 作成端末 
        ///// </summary>
        //public string CreateMachine
        //{
        //    get { return OdrInf.CreateMachine; }
        //    set
        //    {
        //        if (OdrInf.CreateMachine == value) return;
        //        OdrInf.CreateMachine = value;
        //        //RaisePropertyChanged(() => CreateMachine);
        //    }
        //}

        ///// <summary>
        ///// 更新日時 
        ///// </summary>
        //public DateTime UpdateDate
        //{
        //    get { return OdrInf.UpdateDate; }
        //    set
        //    {
        //        if (OdrInf.UpdateDate == value) return;
        //        OdrInf.UpdateDate = value;
        //        //RaisePropertyChanged(() => UpdateDate);
        //    }
        //}

        ///// <summary>
        ///// 更新者
        ///// </summary>
        //public int UpdateId
        //{
        //    get { return OdrInf.UpdateId; }
        //    set
        //    {
        //        if (OdrInf.UpdateId == value) return;
        //        OdrInf.UpdateId = value;
        //        //RaisePropertyChanged(() => UpdateId);
        //    }
        //}

        ///// <summary>
        ///// 更新端末 
        ///// </summary>
        //public string UpdateMachine
        //{
        //    get { return OdrInf.UpdateMachine; }
        //    set
        //    {
        //        if (OdrInf.UpdateMachine == value) return;
        //        OdrInf.UpdateMachine = value;
        //        //RaisePropertyChanged(() => UpdateMachine);
        //    }
        //}

        //public TodayOdrInfModel DeepClone()
        //{
        //    OdrInf newOdrInf = new OdrInf()
        //    {
        //        HpId = this.HpId,
        //        PtId = this.PtId,
        //        SinDate = this.SinDate,
        //        RaiinNo = this.RaiinNo,
        //        RpNo = this.RpNo,
        //        RpEdaNo = this.RpEdaNo,
        //        HokenPid = this.HokenPid,
        //        OdrKouiKbn = this.OdrKouiKbn,
        //        RpName = this.RpName,
        //        InoutKbn = this.InoutKbn,
        //        SikyuKbn = this.SikyuKbn,
        //        SyohoSbt = this.SyohoSbt,
        //        SanteiKbn = this.SanteiKbn,
        //        TosekiKbn = this.TosekiKbn,
        //        DaysCnt = this.DaysCnt,
        //        IsDeleted = this.IsDeleted,
        //        SortNo = this.SortNo
        //    };

        //    List<OdrInfDetail> newOdrDetails = new List<OdrInfDetail>();
        //    foreach (var odrDetail in this.OdrInfDetailModels)
        //    {
        //        OdrInfDetail newOdrDetail = new OdrInfDetail()
        //        {
        //            HpId = odrDetail.HpId,
        //            PtId = odrDetail.PtId,
        //            SinDate = odrDetail.SinDate,
        //            RaiinNo = odrDetail.RaiinNo,
        //            RpNo = odrDetail.RpNo,
        //            RpEdaNo = odrDetail.RpEdaNo,
        //            RowNo = odrDetail.RowNo,
        //            SinKouiKbn = odrDetail.SinKouiKbn,
        //            ItemCd = odrDetail.ItemCd,
        //            ItemName = odrDetail.ItemName,
        //            Suryo = odrDetail.Suryo,
        //            UnitName = odrDetail.UnitName,
        //            UnitSBT = odrDetail.UnitSBT,
        //            TermVal = odrDetail.TermVal,
        //            KohatuKbn = odrDetail.KohatuKbn,
        //            SyohoKbn = odrDetail.SyohoKbn,
        //            SyohoLimitKbn = odrDetail.SyohoLimitKbn,
        //            DrugKbn = odrDetail.DrugKbn,
        //            YohoKbn = odrDetail.YohoKbn,
        //            Kokuji1 = odrDetail.Kokuji1,
        //            Kokiji2 = odrDetail.Kokuji2,
        //            IsNodspRece = odrDetail.IsNodspRece,
        //            IpnCd = odrDetail.IpnCd,
        //            IpnName = odrDetail.IpnName,
        //            JissiKbn = odrDetail.JissiKbn,
        //            JissiDate = odrDetail.JissiDate,
        //            JissiId = odrDetail.JissiId,
        //            JissiMachine = odrDetail.JissiMachine,
        //            ReqCd = odrDetail.ReqCd,
        //            Bunkatu = odrDetail.Bunkatu,
        //            CmtName = odrDetail.CmtName,
        //            CmtOpt = odrDetail.CmtOpt,
        //            FontColor = odrDetail.FontColor,
        //            CommentNewline = odrDetail.CommentNewline
        //        };
        //        newOdrDetails.Add(newOdrDetail);
        //    }

        //    TodayOdrInfModel result = new TodayOdrInfModel(newOdrInf, newOdrDetails);
        //    return result;
        //}

        //#region Exposed properties
        //private ManagedItemCollection<TodayOdrInfDetailModel> _todayOdrInfDetailModels;
        //public ManagedItemCollection<TodayOdrInfDetailModel> OdrInfDetailModels
        //{
        //    get => _todayOdrInfDetailModels;
        //    set
        //    {
        //        if (Set(ref _todayOdrInfDetailModels, value))
        //        {
        //            if (value != null)
        //            {
        //                OdrInfDetailModels.CollectionChanged -= OdrInfDetailModels_CollectionChanged;
        //                OdrInfDetailModels.CollectionChanged += OdrInfDetailModels_CollectionChanged;
        //                foreach (var odrDetail in OdrInfDetailModels)
        //                {
        //                    odrDetail.PropertyChanged -= OdrDetail_PropertyChanged;
        //                    odrDetail.PropertyChanged += OdrDetail_PropertyChanged;
        //                    odrDetail.InOutKbn = this.InoutKbn;
        //                    odrDetail.OdrInfOdrKouiKbn = this.OdrKouiKbn;
        //                }
        //                //RaisePropertyChanged(() => OdrInfTitle);
        //                //RaisePropertyChanged(() => UsingType);
        //                //RaisePropertyChanged(() => DrugPrice);
        //            }
        //            UpdateAlternationIndex();
        //        }
        //    }
        //}

        //private void UpdateAlternationIndex()
        //{
        //    if (OdrInfDetailModels == null) return;
        //    for (int i = 0; i < OdrInfDetailModels.Count(); i++)
        //    {
        //        OdrInfDetailModels[i].AlternationIndex = i % 2;
        //    }
        //}

        //private ManagedItemCollection<TodayOdrInfDetailModel> _odrInfDetailModelView;
        //public ManagedItemCollection<TodayOdrInfDetailModel> OdrInfDetailModelView
        //{
        //    get => _odrInfDetailModelView;
        //    set => Set(ref _odrInfDetailModelView, value);
        //}

        //public ManagedItemCollection<TodayOdrInfDetailModel> OdrInfDetailModelsIgnoreEmpty
        //{
        //    get
        //    {
        //        if (OdrInfDetailModels == null)
        //        {
        //            return OdrInfDetailModels;
        //        }
        //        return new ManagedItemCollection<TodayOdrInfDetailModel>(OdrInfDetailModels.Where(o => !o.IsEmpty).ToList());
        //    }
        //}

        //private TodayOdrInfDetailModel _selectedOrderDetailModel;
        //public TodayOdrInfDetailModel SelectedOrderDetailModel
        //{
        //    get
        //    {
        //        return _selectedOrderDetailModel;
        //    }
        //    set
        //    {
        //        Set(ref _selectedOrderDetailModel, value);
        //    }
        //}

        //private void OdrInfDetailModels_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    if (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Replace)
        //    {
        //        IList newOdrInfDetails = e.NewItems;
        //        if (newOdrInfDetails != null && newOdrInfDetails.Count > 0)
        //        {
        //            foreach (var newOdrInfDetailObj in newOdrInfDetails)
        //            {
        //                TodayOdrInfDetailModel newOdrInfDetail = newOdrInfDetailObj as TodayOdrInfDetailModel;
        //                if (newOdrInfDetail != null)
        //                {
        //                    newOdrInfDetail.PropertyChanged -= OdrDetail_PropertyChanged;
        //                    newOdrInfDetail.PropertyChanged += OdrDetail_PropertyChanged;
        //                }
        //            }
        //        }
        //        //RaisePropertyChanged(() => OdrInfTitle);
        //        //RaisePropertyChanged(() => UsingType);
        //        UpdateAlternationIndex();
        //    }
        //}

        //private void OdrDetail_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    TodayOdrInfDetailModel odrInfDetail = sender as TodayOdrInfDetailModel;
        //    if (odrInfDetail == null) return;
        //    switch (e.PropertyName)
        //    {
        //        case nameof(odrInfDetail.IsShownReleasedDrug):
        //            //RaisePropertyChanged(() => IsShownReleasedDrugColumn);
        //            break;
        //        case nameof(odrInfDetail.ItemName):
        //            //RaisePropertyChanged(() => OdrInfTitle);
        //            break;
        //        case nameof(odrInfDetail.SinKouiKbn):
        //            //RaisePropertyChanged(() => UsingType);
        //            break;
        //    }
        //}

        //public bool IsVisible
        //{
        //    get => IsDeleted == 0;
        //}

        //public bool IsAddNew
        //{
        //    get => Id == 0;
        //}

        //public bool IsShowTitle
        //{
        //    get => UserConfCommon.Instance.DisplaySetName == 1
        //        || UserConfCommon.Instance.DisplayUserInput == 1
        //        || UserConfCommon.Instance.DisplayTimeInput == 1
        //        || UserConfCommon.Instance.DisplayDrugPrice == 1
        //        || !IsExpanded;
        //}

        //public string OdrInfTitle
        //{
        //    get => BuildOdrTitle();
        //}

        //private string BuildOdrTitle()
        //{
        //    string result = string.Empty;
        //    if (!string.IsNullOrEmpty(RpName))
        //    {
        //        result += " 【" + RpName + "】";
        //    }
        //    else
        //    {
        //        if (OdrInfDetailModels.Count > 0)
        //        {
        //            // 項目コード4桁以下は無視する
        //            var odrInfDetail = OdrInfDetailModels.Where(detail => detail.ItemCd?.Length > 4).FirstOrDefault();
        //            if (odrInfDetail != null)
        //            {
        //                result += odrInfDetail.ItemName + "...";
        //            }
        //            else
        //            {
        //                // コメント/部位しかないRpの場合は先頭を
        //                result += OdrInfDetailModels[0].ItemName + "...";
        //            }
        //        }
        //    }

        //    // 入力日時
        //    if (CreateDate != DateTime.MinValue)
        //    {
        //        result += " " + CIUtil.GetCIDateTimeStr(CreateDate);
        //    }
        //    //入力者
        //    if (CreateId != 0)
        //    {
        //        result += " " + UserMstCache.Instance.GetUserSNameByUserId(CreateId);
        //    }

        //    // TODO get NHIPoint

        //    return result;
        //}

        ///// <summary>
        ///// 自己注射 - Self-Injection
        ///// </summary>
        //public bool IsSelfInjection => OdrKouiKbn == 28;

        //// 処方 - Drug
        //public bool IsDrug
        //{
        //    get
        //    {
        //        return OdrKouiKbn >= 21 && OdrKouiKbn <= 23;
        //    }
        //}

        //// 注射 - Injection
        //public bool IsInjection
        //{
        //    get
        //    {
        //        return OdrKouiKbn >= 30 && OdrKouiKbn <= 34;
        //    }
        //}

        //public bool IsShohoComment
        //{
        //    get => OdrKouiKbn == 100;
        //}

        //public bool IsShohoBiko
        //{
        //    get => OdrKouiKbn == 101;
        //}

        //public bool IsShohosenComment
        //{
        //    get => IsShohoComment || IsShohoBiko;
        //}

        //public string TitleUpdate
        //{
        //    get
        //    {
        //        return string.Empty;
        //    }
        //}

        //public string DrugPrice
        //{
        //    get
        //    {
        //        string result = string.Empty;

        //        return result;
        //    }
        //}

        //private bool _isExpanded = true;
        //public bool IsExpanded
        //{
        //    get => _isExpanded;
        //    set
        //    {
        //        Set(ref _isExpanded, value);
        //        this.RaisePropertyChanged(() => this.IsShowCollapsedTitle);
        //    }
        //}

        //public bool IsShowCollapsedTitle
        //{
        //    get => !this._isExpanded;
        //}

        //private bool _isShownCheckbox = false;
        //public bool IsShownCheckbox
        //{
        //    get => _isShownCheckbox;
        //    set
        //    {
        //        Set(ref _isShownCheckbox, value);
        //    }
        //}

        //private bool _isChecked = false;
        //public bool IsChecked
        //{
        //    get => _isChecked;
        //    set
        //    {
        //        if (Set(ref _isChecked, value))
        //        {
        //            foreach (var odrInfDetail in OdrInfDetailModels)
        //            {
        //                odrInfDetail.IsChecked = value;
        //            }
        //        }
        //    }
        //}

        //private bool _isSelected;
        //public bool IsSelected
        //{
        //    get
        //    {
        //        return this._isSelected;
        //    }
        //    set
        //    {
        //        Set(ref this._isSelected, value);
        //    }
        //}

        //public string GUID { get; } = Guid.NewGuid().ToString();

        //public UsingDrugType UsingType
        //{
        //    get
        //    {
        //        if (IsDrug || IsInjection)
        //        {
        //            foreach (TodayOdrInfDetailModel todayOdrInfDetailModel in this.OdrInfDetailModels)
        //            {
        //                if (todayOdrInfDetailModel.SinKouiKbn == OdrKouiKbnConst.Naifuku)
        //                {
        //                    return UsingDrugType.Drinking;
        //                }
        //                else if (todayOdrInfDetailModel.SinKouiKbn == OdrKouiKbnConst.Tonpuku)
        //                {
        //                    return UsingDrugType.AsOrder;
        //                }
        //                else if (todayOdrInfDetailModel.SinKouiKbn == OdrKouiKbnConst.Gaiyo)
        //                {
        //                    return UsingDrugType.OnSkin;
        //                }
        //            }
        //        }
        //        return UsingDrugType.None;
        //    }
        //}

        //private bool _isShownReleasedDrugColumn;
        //public bool IsShownReleasedDrugColumn
        //{
        //    get => _isShownReleasedDrugColumn;
        //    set => Set(ref _isShownReleasedDrugColumn, value);
        //}

        //private bool _isShownQuantityColumn;
        //public bool IsShownQuantityColumn
        //{
        //    get => _isShownQuantityColumn;
        //    set => Set(ref _isShownQuantityColumn, value);
        //}

        //private bool _isShownUnitColumn;
        //public bool IsShownUnitColumn
        //{
        //    get => _isShownUnitColumn;
        //    set => Set(ref _isShownUnitColumn, value);
        //}

        //private double _itemNameColumnWidth;
        //public double ItemNameColumnWidth
        //{
        //    get => _itemNameColumnWidth;
        //    set => Set(ref _itemNameColumnWidth, value);
        //}

        //private double _quantityColumnWidth;
        //public double QuantityColumnWidth
        //{
        //    get => _quantityColumnWidth;
        //    set => Set(ref _quantityColumnWidth, value);
        //}

        //private double _unitColumnWidth;
        //public double UnitColumnWidth
        //{
        //    get => _unitColumnWidth;
        //    set => Set(ref _unitColumnWidth, value);
        //}

        //private double _releasedDrugColumnWidth;
        //public double ReleasedDrugColumnWidth
        //{
        //    get => _releasedDrugColumnWidth;
        //    set => Set(ref _releasedDrugColumnWidth, value);
        //}

        //public bool IsItemNameReadOnly => false;

        //public bool IsQuantityReadOnly => false;

        //public bool IsShowKensaGaichu => true;

        //public bool IsSameGroupWith(TodayOdrInfModel other)
        //{
        //    return this.HokenPid == other.HokenPid
        //        && this.GroupOdrKouiKbn == other.GroupOdrKouiKbn
        //        && this.InoutKbn == other.InoutKbn
        //        && this.SikyuKbn == other.SikyuKbn
        //        && this.TosekiKbn == other.TosekiKbn
        //        && this.SyohoSbt == other.SyohoSbt
        //        && this.SanteiKbn == other.SanteiKbn;
        //}
        //#endregion
    }
}
