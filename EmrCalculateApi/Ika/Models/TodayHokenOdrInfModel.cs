using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;


namespace EmrCalculateApi.Ika.Models
{
    public class TodayHokenOdrInfModel 
    {
        //private ThreadSafeCollection<TodayGroupOdrInfModel> _todayGroupOdrInfModels;
        //public ThreadSafeCollection<TodayGroupOdrInfModel> GroupOdrInfModels
        //{
        //    get => _todayGroupOdrInfModels;
        //    set
        //    {
        //        if (Set(ref _todayGroupOdrInfModels, value))
        //        {
        //            if (value != null)
        //            {
        //                GroupOdrInfModels.CollectionChanged -= GroupOdrInfModels_CollectionChanged;
        //                GroupOdrInfModels.CollectionChanged += GroupOdrInfModels_CollectionChanged;

        //                foreach (var grpOdrInf in GroupOdrInfModels)
        //                {
        //                    grpOdrInf.PropertyChanged -= GroupOdr_PropertyChanged;
        //                    grpOdrInf.PropertyChanged += GroupOdr_PropertyChanged;
        //                }

        //                HokenGroupOdrInfModelView = CollectionViewSource.GetDefaultView(value) as ListCollectionView;

        //                HokenGroupOdrInfModelView.CustomSort = new GroupOdrSorter<TodayGroupOdrInfModel, TodayOdrInfModel, TodayOdrInfDetailModel>();

        //                HokenGroupOdrInfModelView.Filter += VisibleFilter;

        //                //RaisePropertyChanged(() => IsVisible);
        //            }
        //        }
        //    }
        //}

        //private ListCollectionView _hokenGroupOdrInfModelView;
        //public ListCollectionView HokenGroupOdrInfModelView
        //{
        //    get => _hokenGroupOdrInfModelView;
        //    set
        //    {
        //        Set(ref _hokenGroupOdrInfModelView, value);
        //    }
        //}

        //private bool VisibleFilter(object item)
        //{
        //    TodayGroupOdrInfModel odrInfModel = item as TodayGroupOdrInfModel;
        //    return odrInfModel.IsVisible;
        //}

        //private void GroupOdrInfModels_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    if (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Replace)
        //    {
        //        IList newGroupOdrs = e.NewItems;
        //        if (newGroupOdrs != null && newGroupOdrs.Count > 0)
        //        {
        //            TodayGroupOdrInfModel newGroupOdr = newGroupOdrs[0] as TodayGroupOdrInfModel;
        //            if (newGroupOdr != null)
        //            {
        //                newGroupOdr.PropertyChanged -= GroupOdr_PropertyChanged;
        //                newGroupOdr.PropertyChanged += GroupOdr_PropertyChanged;
        //            }
        //        }
        //    }
        //    if (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Remove
        //        || e.Action == NotifyCollectionChangedAction.Replace || e.Action == NotifyCollectionChangedAction.Reset)
        //    {
        //        //RaisePropertyChanged(() => IsVisible);
        //    }
        //}

        //private void GroupOdr_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    TodayGroupOdrInfModel groupOdrInf = sender as TodayGroupOdrInfModel;
        //    if (groupOdrInf == null) return;
        //    switch (e.PropertyName)
        //    {
        //        case nameof(groupOdrInf.IsVisible):
        //            //RaisePropertyChanged(() => IsVisible);
        //            break;
        //        case nameof(groupOdrInf.IsExist):
        //            if (!groupOdrInf.IsExist)
        //            {
        //                GroupOdrInfModels.Remove(groupOdrInf);
        //            }
        //            //RaisePropertyChanged(() => IsExist);
        //            break;
        //        default:
        //            RaiseAllPropertiesChanged();
        //            break;
        //    }
        //}

        //private int _mainPatternPid;
        //public int MainPatternPid
        //{
        //    get => _mainPatternPid;
        //    set
        //    {
        //        if (Set(ref _mainPatternPid, value))
        //        {
        //            //RaisePropertyChanged(() => IsShowHokenRow);
        //        }
        //    }
        //}

        //private PtHokenPatternModel _hokenPatternModel;
        //public PtHokenPatternModel HokenPatternModel
        //{
        //    get => _hokenPatternModel;
        //    set
        //    {
        //        if (Set(ref _hokenPatternModel, value))
        //        {
        //            if (value != null)
        //            {
        //                PatternPid = value.HokenPid;
        //            }
        //            else
        //            {
        //                PatternPid = 0;
        //            }
        //        }
        //    }
        //}

        //public int PatternPid
        //{
        //    get
        //    {
        //        var firstVisibleGroup = GroupOdrInfModels.Where(g => g.IsVisible).FirstOrDefault();
        //        if (firstVisibleGroup != null)
        //        {
        //            return firstVisibleGroup.HokenPid;
        //        }
        //        return -1;
        //    }
        //    set
        //    {
        //        if (PatternPid == value) return;
        //        foreach (var groupOdr in GroupOdrInfModels)
        //        {
        //            groupOdr.HokenPid = value;
        //        }
        //        //RaisePropertyChanged(() => PatternPid);
        //        //RaisePropertyChanged(() => IsShowHokenRow);
        //    }
        //}

        //public bool IsShowHokenRow
        //{
        //    get => MainPatternPid != PatternPid;
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
        //    get => GroupOdrInfModels.Where(odr => odr.IsVisible).Count() > 0;
        //}

        //public bool IsExist
        //{
        //    get => GroupOdrInfModels.Count() > 0;
        //}

        //public TodayHokenOdrInfModel()
        //{
        //    GroupOdrInfModels = new ThreadSafeCollection<TodayGroupOdrInfModel>();
        //}

        //public TodayHokenOdrInfModel(List<TodayGroupOdrInfModel> todayGroupOdrInfModels)
        //{
        //    GroupOdrInfModels = new ThreadSafeCollection<TodayGroupOdrInfModel>(todayGroupOdrInfModels);
        //}
    }
}
