using Domain.Models.ReceptionSameVisit;
using Entity.Tenant;
using Helper.Common;
using Helper.Extendsions;
using System.Xml.Linq;

namespace Domain.Models.RaiinFilterMst;

public class RaiinFilterMstModel : ObservableObject
{
    public RaiinInf? RaiinInf { get; } = null;
    public HokenPatternModel? HokenPatternModel { get; } = null;
    public RaiinFilterMstModel(int filterId, int sortNo, string filterName,
        int selectKbn, string shortcut, List<RaiinFilterSortModel> columnSortInfos)
    {
        FilterId = filterId;
        SortNo = sortNo;
        FilterName = filterName;
        SelectKbn = selectKbn;
        Shortcut = shortcut;
        ColumnSortInfos = columnSortInfos;
    }
    public RaiinFilterMstModel(RaiinInf raiinInf, HokenPatternModel hokenPatternModel, string sName, string kaSname) 
    {
        RaiinInf = raiinInf;
        HokenPatternModel = hokenPatternModel;
        Sname = sName;
        KaSname = kaSname;
    }

    public string Sname { get; private set; }
    public string KaSname { get; private set; }

    public int FilterId { get; private set; }
    public int SortNo { get; private set; }
    public string FilterName { get; private set; }

    /// <summary>
    ///     0: 状態に応じた画面を開く
    ///     1: 属性編集
    ///     2: カルテ作成
    ///     3: 窓口精算
    /// </summary>
    public int SelectKbn { get; private set; }
    public string Shortcut { get; private set; }
    public bool CheckDefaultValue()
    {
        return RaiinInf?.PtId == 0 && RaiinInf?.SinDate == 0 && RaiinInf?.RaiinNo == 0;
    }
    public long RaiinNo
    {
        get { return RaiinInf.RaiinNo; }
        set
        {
            if (RaiinInf?.RaiinNo == value) return;
            RaiinInf.RaiinNo = value;
            RaisePropertyChanged(() => RaiinNo);
        }
    }
    public string VisitingInf
    {
        get { return SinDateLabel + "［" + KaSname + "/" + Sname + "］"; }
    }
    public int SinDate
    {
        get { return RaiinInf.SinDate; }
        set
        {
            if (RaiinInf?.SinDate == value) return;
            RaiinInf.SinDate = value;
            RaisePropertyChanged(() => SinDate);
            RaisePropertyChanged(() => SinDateLabel);
            RaisePropertyChanged(() => VisitingInf);
        }
    }
    public string SinDateLabel
    {
        get { return CIUtil.SDateToShowSDate(SinDate); }
    }

    public int Status
    {
        get { return RaiinInf.Status; }
        set
        {
            if (RaiinInf?.Status == value) return;
            RaiinInf.Status = value;
            RaisePropertyChanged(() => Status);
            RaisePropertyChanged(() => StatusLbl);
        }
    }
    public string StatusLbl
    {
        get
        {
            if (CheckDefaultValue())
            {
                return string.Empty;
            }
            string result = string.Empty;
            switch (Status)
            {
                case 0:
                    result = "予約";
                    break;
                case 1:
                    result = "";
                    break;
                case 3:
                    result = "一時保存";
                    break;
                case 5:
                    result = "計算";
                    break;
                case 7:
                    result = "精算待ち";
                    break;
                case 9:
                    result = "精算済";
                    break;
                default:
                    break;
            }
            return result;
        }
    }
    public List<RaiinFilterSortModel> ColumnSortInfos { get; private set; }
}
