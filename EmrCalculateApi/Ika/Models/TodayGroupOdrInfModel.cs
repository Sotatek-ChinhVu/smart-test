using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;


namespace EmrCalculateApi.Ika.Models
{
    public class TodayGroupOdrInfModel 
    {
        //private ManagedItemCollection<TodayOdrInfModel> _todayOdrInfModels;
        //public ManagedItemCollection<TodayOdrInfModel> OdrInfModels
        //{
        //    get => _todayOdrInfModels;
        //    set
        //    {
        //        if (Set(ref _todayOdrInfModels, value))
        //        {
        //            if (value != null)
        //            {
        //                OdrInfModels.CollectionChanged -= OdrInfModels_CollectionChanged;
        //                OdrInfModels.CollectionChanged += OdrInfModels_CollectionChanged;

        //                foreach (var odrInf in OdrInfModels)
        //                {
        //                    odrInf.PropertyChanged -= OdrInf_PropertyChanged;
        //                    odrInf.PropertyChanged += OdrInf_PropertyChanged;
        //                }

        //                //RaisePropertyChanged(() => IsVisible);
        //            }
        //        }
        //    }
        //}

        //private TodayOdrInfModel _selectedOrderModel;
        //public TodayOdrInfModel SelectedOrderModel
        //{
        //    get
        //    {
        //        return _selectedOrderModel;
        //    }
        //    set
        //    {
        //        Set(ref _selectedOrderModel, value);
        //    }
        //}

        //private void OdrInfModels_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    if (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Replace)
        //    {
        //        IList newOdrInfs = e.NewItems;
        //        if (newOdrInfs != null && newOdrInfs.Count > 0)
        //        {
        //            foreach (var newOdrInfObj in newOdrInfs)
        //            {
        //                TodayOdrInfModel newOdrInf = newOdrInfObj as TodayOdrInfModel;
        //                if (newOdrInf != null)
        //                {
        //                    newOdrInf.PropertyChanged -= OdrInf_PropertyChanged;
        //                    newOdrInf.PropertyChanged += OdrInf_PropertyChanged;
        //                }
        //            }
        //        }
        //    }
        //    if (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Remove
        //        || e.Action == NotifyCollectionChangedAction.Replace || e.Action == NotifyCollectionChangedAction.Reset)
        //    {
        //        //RaisePropertyChanged(() => IsVisible);
        //        //RaisePropertyChanged(() => IsExist);
        //    }
        //}

        ///// <summary>
        ///// Apply for each OdrInfModel
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void OdrInf_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    TodayOdrInfModel odrInfModel = sender as TodayOdrInfModel;
        //    if (odrInfModel == null) return;
        //    switch (e.PropertyName)
        //    {
        //        case nameof(odrInfModel.IsDeleted):
        //            //RaisePropertyChanged(() => IsVisible);
        //            break;
        //        case nameof(odrInfModel.InoutKbn):
        //            //RaisePropertyChanged(() => InOutKbn);
        //            //RaisePropertyChanged(() => InOutName);
        //            //RaisePropertyChanged(() => IsShowInOut);
        //            break;
        //        case nameof(odrInfModel.SanteiKbn):
        //            //RaisePropertyChanged(() => SanteiKbn);
        //            //RaisePropertyChanged(() => SanteiName);
        //            //RaisePropertyChanged(() => IsShowSantei);
        //            break;
        //        case nameof(odrInfModel.SyohoSbt):
        //            //RaisePropertyChanged(() => SyohoSbt);
        //            //RaisePropertyChanged(() => SikyuName);
        //            //RaisePropertyChanged(() => IsShowSikyu);
        //            break;
        //        case nameof(odrInfModel.SikyuKbn):
        //            //RaisePropertyChanged(() => SikyuKbn);
        //            //RaisePropertyChanged(() => SikyuName);
        //            //RaisePropertyChanged(() => IsShowSikyu);
        //            break;
        //        case nameof(odrInfModel.TosekiKbn):
        //            //RaisePropertyChanged(() => TosekiKbn);
        //            //RaisePropertyChanged(() => SikyuName);
        //            //RaisePropertyChanged(() => IsShowSikyu);
        //            break;
        //    }
        //}

        //public TodayOdrInfModel FirstOdrInfModel
        //{
        //    get
        //    {
        //        return OdrInfModels.Where(odr => odr.IsVisible).FirstOrDefault();
        //    }
        //}

        //public int KouiCode
        //{
        //    get
        //    {
        //        TodayOdrInfModel firstTodayOdrInfModel = FirstOdrInfModel;
        //        if (firstTodayOdrInfModel != null)
        //        {
        //            return firstTodayOdrInfModel.OdrKouiKbn;
        //        }
        //        return 0;
        //    }
        //}

        //public int GroupKouiCode
        //{
        //    get
        //    {
        //        TodayOdrInfModel firstTodayOdrInfModel = FirstOdrInfModel;
        //        if (firstTodayOdrInfModel != null)
        //        {
        //            return firstTodayOdrInfModel.GroupOdrKouiKbn;
        //        }
        //        return 0;
        //    }
        //}

        //public string GroupName
        //{
        //    get
        //    {
        //        TodayOdrInfModel firstTodayOdrInfModel = FirstOdrInfModel;
        //        if (firstTodayOdrInfModel != null)
        //        {
        //            return OdrUtil.GetOdrGroupName(firstTodayOdrInfModel.OdrKouiKbn);
        //        }
        //        return "";
        //    }
        //}

        //public bool IsShowInOut
        //{
        //    get => !string.IsNullOrEmpty(InOutName);
        //}

        //public int InOutKbn
        //{
        //    get
        //    {
        //        TodayOdrInfModel firstTodayOdrInfModel = FirstOdrInfModel;
        //        if (firstTodayOdrInfModel != null)
        //        {
        //            return firstTodayOdrInfModel.InoutKbn;
        //        }
        //        return 0;
        //    }
        //}

        //public string InOutName
        //{
        //    get
        //    {
        //        TodayOdrInfModel firstTodayOdrInfModel = FirstOdrInfModel;
        //        if (firstTodayOdrInfModel != null)
        //        {
        //            return OdrUtil.GetInOutName(firstTodayOdrInfModel.OdrKouiKbn, firstTodayOdrInfModel.InoutKbn);
        //        }
        //        return "";
        //    }
        //}

        //public bool IsShowSikyu
        //{
        //    get
        //    {
        //        string sikyuName = SikyuName;
        //        return !string.IsNullOrEmpty(sikyuName)
        //            && sikyuName != "日数" // in case 日数, dosen't need display, Comment #375
        //            && sikyuName != "通常";
        //    }
        //}

        //public int SikyuKbn
        //{
        //    get
        //    {
        //        TodayOdrInfModel firstTodayOdrInfModel = FirstOdrInfModel;
        //        if (firstTodayOdrInfModel != null)
        //        {
        //            return firstTodayOdrInfModel.SikyuKbn;
        //        }
        //        return 0;
        //    }
        //}

        //public int TosekiKbn
        //{
        //    get
        //    {
        //        TodayOdrInfModel firstTodayOdrInfModel = FirstOdrInfModel;
        //        if (firstTodayOdrInfModel != null)
        //        {
        //            return firstTodayOdrInfModel.TosekiKbn;
        //        }
        //        return 0;
        //    }
        //}

        //public int SyohoSbt
        //{
        //    get
        //    {
        //        TodayOdrInfModel firstTodayOdrInfModel = FirstOdrInfModel;
        //        if (firstTodayOdrInfModel != null)
        //        {
        //            return firstTodayOdrInfModel.SyohoSbt;
        //        }
        //        return 0;
        //    }
        //}

        //public string SikyuName
        //{
        //    get
        //    {
        //        if (IsDrug)
        //        {
        //            return OdrUtil.GetSikyuName(SyohoSbt);
        //        }
        //        else if (IsKensa)
        //        {
        //            return OdrUtil.GetSikyuKensa(SikyuKbn, TosekiKbn);
        //        }
        //        return "";
        //    }
        //}

        //public bool IsDrug
        //{
        //    get => (KouiCode >= 20 && KouiCode <= 23) || KouiCode == 28 || KouiCode == 100 || KouiCode == 101;
        //}

        //public bool IsInjection
        //{
        //    get => KouiCode >= 30 && KouiCode <= 34;
        //}

        //public bool IsKensa
        //{
        //    get => (KouiCode >= 60 && KouiCode < 70);
        //}

        //public int HokenPid
        //{
        //    get
        //    {
        //        TodayOdrInfModel firstTodayOdrInfModel = FirstOdrInfModel;
        //        if (firstTodayOdrInfModel != null)
        //        {
        //            return firstTodayOdrInfModel.HokenPid;
        //        }
        //        return 0;
        //    }
        //    set
        //    {
        //        foreach (TodayOdrInfModel todayOdrInfModel in OdrInfModels)
        //        {
        //            todayOdrInfModel.HokenPid = value;
        //        }
        //        //RaisePropertyChanged(() => HokenPid);
        //    }
        //}

        //public bool IsShowSantei
        //{
        //    get => !string.IsNullOrEmpty(SanteiName);
        //}

        //public int SanteiKbn
        //{
        //    get
        //    {
        //        TodayOdrInfModel firstTodayOdrInfModel = FirstOdrInfModel;
        //        if (firstTodayOdrInfModel != null)
        //        {
        //            return firstTodayOdrInfModel.SanteiKbn;
        //        }
        //        return 0;
        //    }
        //}

        //public string SanteiName
        //{
        //    get
        //    {
        //        if (SanteiKbn == 1)
        //        {
        //            return "算定外";
        //        }
        //        else if (SanteiKbn == 2)
        //        {
        //            return "自費算定";
        //        }
        //        return "";
        //    }
        //}

        //private bool _isExpanded = true;
        //public bool IsExpanded
        //{
        //    get => _isExpanded;
        //    set
        //    {
        //        Set(ref _isExpanded, value);
        //    }
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
        //            foreach (var odrInf in OdrInfModels)
        //            {
        //                odrInf.IsChecked = value;
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

        //public bool IsVisible
        //{
        //    get => OdrInfModels.Where(odr => odr.IsVisible).Count() > 0;
        //}

        //public bool IsExist
        //{
        //    get => OdrInfModels.Count() > 0;
        //}

        //public string GUID { get; } = Guid.NewGuid().ToString();

        //public TodayGroupOdrInfModel()
        //{
        //    OdrInfModels = new ManagedItemCollection<TodayOdrInfModel>();
        //}

        //public TodayGroupOdrInfModel(List<TodayOdrInfModel> todayOdrInfModels)
        //{
        //    OdrInfModels = new ManagedItemCollection<TodayOdrInfModel>(todayOdrInfModels);
        //}
    }
}
