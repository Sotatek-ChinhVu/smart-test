using Domain.Models.DrugInfor;
using Domain.Models.UserConf;
using Helper.Common;
using Helper.Extension;
using UseCase.DrugInfor.GetContentDrugUsageHistory;
using UseCase.DrugInfor.Model;

namespace Interactor.DrugInfor;

public class GetContentDrugUsageHistoryInteractor : IGetContentDrugUsageHistoryInputPort
{
    private readonly IDrugInforRepository _drugInforRepository;
    private readonly IUserConfRepository _userConfRepository;
    private readonly List<int> kouiKbnIdList = new() { 23, 1, 2, 24, 3, 4, 5, 6, 25, 11, 26, 27, 17, 28, 22 };

    public GetContentDrugUsageHistoryInteractor(IDrugInforRepository drugInforRepository, IUserConfRepository userConfRepository)
    {
        _drugInforRepository = drugInforRepository;
        _userConfRepository = userConfRepository;
    }

    public GetContentDrugUsageHistoryOutputData Handle(GetContentDrugUsageHistoryInputData inputData)
    {
        try
        {
            var drugUsageHistory = GetData(inputData.HpId, inputData.UserId, inputData.PtId, inputData.GrpCd, inputData.StartDate, inputData.EndDate);
            return new GetContentDrugUsageHistoryOutputData(drugUsageHistory, GetContentDrugUsageHistoryStatus.Successed);
        }
        finally
        {
            _drugInforRepository.ReleaseResource();
            _userConfRepository.ReleaseResource();
        }
    }

    private List<DrugUsageHistoryGroupModel> GetData(int hpId, int userId, long ptId, int grpId, int startDate, int endDate, int mode = 0)
    {
        List<DrugUsageHistoryContentModel> drugUsageHistory = new();
        var drugUsageHistoryList = _drugInforRepository.GetDrugUsageHistoryList(hpId, ptId);
        if (mode == 0)
        {
            drugUsageHistoryList = drugUsageHistoryList.Where(item => item.SinDate >= startDate && item.SinDate <= endDate
                                                                      || item.OdrKouiKbn == 21 && item.SinDate < startDate && item.EndDate >= startDate)
                                                       .ToList();
        }
        else
        {
            drugUsageHistoryList = drugUsageHistoryList.Where(item => item.SinDate >= startDate && item.SinDate <= endDate).ToList();
        }
        var allKouiKbnMstList = _drugInforRepository.GetKouiKbnMstList(hpId);
        if (grpId > 0)
        {
            var sinrekiFilter = _drugInforRepository.GetSinrekiFilterMst(hpId, grpId);
            drugUsageHistoryList = FilterData(drugUsageHistoryList, sinrekiFilter.SinrekiFilterMstKouiList, allKouiKbnMstList);
            drugUsageHistoryList = FilterData(drugUsageHistoryList, sinrekiFilter.SinrekiFilterMstDetailList);
        }

        var drugUsageHistoryDistinctList = GetDataDistinct(drugUsageHistoryList);
        foreach (var item in drugUsageHistoryDistinctList)
        {
            string quantity = item.Quantity > 0 && !string.IsNullOrEmpty(item.UnitName) ? item.Quantity.AsString() : string.Empty;
            string unitName = item.Quantity > 0 && !string.IsNullOrEmpty(item.UnitName) ? item.UnitName : string.Empty;
            List<ActionGraph> actionGraphList;
            if (mode == 0)
            {
                actionGraphList = GetListActionGraph(item, drugUsageHistoryList, startDate, endDate);
            }
            else
            {
                actionGraphList = GetListActionTimeSeries(item, drugUsageHistoryList, startDate, endDate);
            }
            drugUsageHistory.Add(new DrugUsageHistoryContentModel(
                                     item.ItemCd,
                                     item.ItemName,
                                     quantity,
                                     unitName,
                                     item.OdrKouiKbn,
                                     item.Suryo,
                                     item.UnitSbt,
                                     actionGraphList));
        }

        var kouiKbnGroupList = allKouiKbnMstList.Where(item => kouiKbnIdList.Contains(item.KouiKbnId)).ToList();
        var result = GroupItemByOdrKouiKbn(kouiKbnGroupList.Where(item => item.OyaKouiKbnId > 0).ToList(), ref drugUsageHistory);
        if (drugUsageHistory.Any())
        {
            result.AddRange(GroupItemByOdrKouiKbn(kouiKbnGroupList.Where(item => item.OyaKouiKbnId == 0).ToList(), ref drugUsageHistory));
        }

        // SortData
        return SortData(hpId, userId, result);
    }

    private List<DrugUsageHistoryModel> FilterData(List<DrugUsageHistoryModel> listDrugUsageHistory, List<SinrekiFilterMstKouiModel> sinrekiFilterMstKouiList, List<KouiKbnMstModel> kouiKbnMstList)
    {
        var kouiKbnFilter = GetListKouiKbnFilter(sinrekiFilterMstKouiList, kouiKbnMstList);

        return listDrugUsageHistory.Where(item => kouiKbnFilter.Contains(item.OdrKouiKbn)).ToList();
    }

    private List<DrugUsageHistoryModel> FilterData(List<DrugUsageHistoryModel> listDrugUsageHistory, List<SinrekiFilterMstDetailModel> sinrekiFilterMstDetailList)
    {
        var itemCdList = sinrekiFilterMstDetailList.Where(item => !item.IsExclude).Select(item => item.ItemCd).Distinct().ToList();
        listDrugUsageHistory = listDrugUsageHistory.Where(item => itemCdList.Contains(item.ItemCd)).ToList();
        return listDrugUsageHistory;
    }

    private List<int> GetListKouiKbnFilter(List<SinrekiFilterMstKouiModel> sinrekiFilterMstKouiList, List<KouiKbnMstModel> kouiKbnMstList)
    {
        List<int> result = new();
        foreach (var kouiFilter in sinrekiFilterMstKouiList)
        {
            var kouiKbnMst = kouiKbnMstList.FirstOrDefault(item => item.KouiKbnId == kouiFilter.KouiKbnId);
            if (kouiKbnMst == null)
            {
                continue;
            }
            var kouiKbn1 = kouiKbnMst.KouiKbn1;
            var kouiKbn2 = kouiKbnMst.KouiKbn2;
            List<int> kouiKbnList = new();
            for (int i = kouiKbn1; i <= kouiKbn2; i++)
            {
                if (kouiKbnMst.ExcKouiKbn != 0 && kouiKbnMst.ExcKouiKbn == i)
                {
                    continue;
                }
                kouiKbnList.Add(i);
            }
            result.AddRange(kouiKbnList);
        }
        result = result.Distinct().ToList();
        return result;
    }

    private List<DrugUsageHistoryModel> GetDataDistinct(List<DrugUsageHistoryModel> drugUsageHistoryList)
    {
        List<DrugUsageHistoryModel> result = new();

        foreach (var drugUsage in drugUsageHistoryList)
        {
            if (string.IsNullOrEmpty(drugUsage.ItemCd))
            {
                continue;
            }
            if (!result.Any(item => item.ItemCd == drugUsage.ItemCd
                                    && item.OdrKouiKbn == drugUsage.OdrKouiKbn
                                    && item.Quantity.AsString() == drugUsage.Quantity.AsString()))
            {
                result.Add(drugUsage);
            }
        }

        return result;
    }

    private List<ActionGraph> GetListActionGraph(DrugUsageHistoryModel drugUsageHistory, List<DrugUsageHistoryModel> listDrugUsageHistory, int fromDate, int toDate)
    {
        List<ActionGraph> actionGraphList = new();
        List<DrugUsageHistoryModel> drugUsageList;

        if (drugUsageHistory.OdrKouiKbn == 21)
        {
            drugUsageList = listDrugUsageHistory.Where(item => item.SinDate < fromDate
                                                               && item.EndDate >= fromDate
                                                               && item.OdrKouiKbn == drugUsageHistory.OdrKouiKbn
                                                               && item.ItemCd == drugUsageHistory.ItemCd
                                                               && item.Quantity.AsString() == drugUsageHistory.Quantity.AsString())
                                                .OrderByDescending(item => item.EndDate)
                                                .ToList();

            foreach (var drugUsage in drugUsageList)
            {
                actionGraphList.Add(new ActionGraph(
                                        ActionType.Nai,
                                        drugUsage.SinDate,
                                        fromDate,
                                        drugUsage.EndDate,
                                        drugUsage.DaysCnt,
                                        string.Format($"{CIUtil.SDateToShowSDate(drugUsage.SinDate)} ～ {CIUtil.SDateToShowSDate(drugUsage.EndDate)} ({drugUsage.DaysCnt}日間)")));
            }
        }

        drugUsageList = listDrugUsageHistory.Where(item => item.SinDate >= fromDate
                                                               && item.SinDate <= toDate
                                                               && item.OdrKouiKbn == drugUsageHistory.OdrKouiKbn
                                                               && item.ItemCd == drugUsageHistory.ItemCd
                                                               && item.Quantity.AsString() == drugUsageHistory.Quantity.AsString())
                                                .OrderBy(item => item.SinDate)
                                                .ToList();

        foreach (var drugUsage in drugUsageList)
        {
            if (drugUsageHistory.OdrKouiKbn == 21)
            {
                actionGraphList.Add(new ActionGraph(
                                        ActionType.Nai,
                                        drugUsage.SinDate,
                                        fromDate,
                                        drugUsage.EndDate,
                                        drugUsage.DaysCnt,
                                        string.Format($"{CIUtil.SDateToShowSDate(drugUsage.SinDate)} ～ {CIUtil.SDateToShowSDate(drugUsage.EndDate)} ({drugUsage.DaysCnt}日間)")));
            }
            else if (drugUsageHistory.OdrKouiKbn == 22)
            {
                actionGraphList.Add(new ActionGraph(
                                        ActionType.Gai,
                                        drugUsage.SinDate,
                                        fromDate,
                                        drugUsage.EndDate,
                                        drugUsage.DaysCnt,
                                        string.Format($"{CIUtil.SDateToShowSDate(drugUsage.SinDate)} ({drugUsage.DaysCnt}回)")));
            }
            else if (drugUsageHistory.OdrKouiKbn == 23)
            {
                actionGraphList.Add(new ActionGraph(
                                        ActionType.Ton,
                                        drugUsage.SinDate,
                                        fromDate,
                                        drugUsage.EndDate,
                                        drugUsage.DaysCnt,
                                        string.Format($"{CIUtil.SDateToShowSDate(drugUsage.SinDate)} ({drugUsage.DaysCnt}回)")));
            }
            else
            {
                if (!string.IsNullOrEmpty(drugUsage.ItemCd))
                {
                    actionGraphList.Add(new ActionGraph(
                                            ActionType.Item,
                                            drugUsage.SinDate,
                                            fromDate,
                                            drugUsage.EndDate,
                                            drugUsage.DaysCnt,
                                            CIUtil.SDateToShowSDate(drugUsage.SinDate)));
                }
            }
        }
        return actionGraphList;
    }

    private List<ActionGraph> GetListActionTimeSeries(DrugUsageHistoryModel drugUsageHistory, List<DrugUsageHistoryModel> listDrugUsageHistory, int fromDate, int toDate)
    {
        List<ActionGraph> actionGraphList = new();

        var listDrugUsage = listDrugUsageHistory.Where(item => item.SinDate >= fromDate &&
                                                               item.SinDate <= toDate &&
                                                               item.OdrKouiKbn == drugUsageHistory.OdrKouiKbn &&
                                                               item.ItemCd == drugUsageHistory.ItemCd &&
                                                               item.Quantity.AsString() == drugUsageHistory.Quantity.AsString())
                                                .OrderByDescending(item => item.SinDate)
                                                .ToList();

        foreach (var drugUsage in listDrugUsage)
        {
            if (drugUsageHistory.OdrKouiKbn == 21)
            {
                actionGraphList.Add(new ActionGraph(
                                        ActionType.TimeSeriesDrug,
                                        drugUsage.SinDate,
                                        fromDate,
                                        drugUsage.EndDate,
                                        drugUsage.DaysCnt,
                                        string.Format($"{CIUtil.SDateToShowSDate(drugUsage.SinDate)} ～ {CIUtil.SDateToShowSDate(drugUsage.EndDate)} ({drugUsage.DaysCnt}日間)")));
            }
            else if (!string.IsNullOrEmpty(drugUsage.ItemCd))
            {
                if (drugUsage.DaysCnt > 1)
                {
                    actionGraphList.Add(new ActionGraph(
                                            ActionType.TimeSeriesMultiDay,
                                            drugUsage.SinDate,
                                            fromDate,
                                            drugUsage.EndDate,
                                            drugUsage.DaysCnt,
                                            string.Format($"{CIUtil.SDateToShowSDate(drugUsage.SinDate)} ({drugUsage.DaysCnt}回)")));
                }
                else
                {
                    actionGraphList.Add(new ActionGraph(
                                            ActionType.TimeSeries,
                                            drugUsage.SinDate,
                                            fromDate,
                                            drugUsage.EndDate,
                                            drugUsage.DaysCnt,
                                            CIUtil.SDateToShowSDate(drugUsage.SinDate)));
                }
            }
        }
        return actionGraphList;
    }

    private List<DrugUsageHistoryGroupModel> GroupItemByOdrKouiKbn(List<KouiKbnMstModel> kouiKbnMstList, ref List<DrugUsageHistoryContentModel> drugUsageHistory)
    {
        List<DrugUsageHistoryGroupModel> result = new();
        foreach (var kouiKbnMst in kouiKbnMstList)
        {
            var drugUsageHistoryContentList = drugUsageHistory.Where(item => item.OdrKouiKbn >= kouiKbnMst.KouiKbn1 && item.OdrKouiKbn <= kouiKbnMst.KouiKbn2 && kouiKbnMst.ExcKouiKbn != item.OdrKouiKbn).ToList();
            if (drugUsageHistoryContentList.Any())
            {
                result.Add(new DrugUsageHistoryGroupModel(
                               kouiKbnMst.KouiKbnId,
                               kouiKbnMst.KouiName,
                               kouiKbnMst.KouiKbn2,
                               drugUsageHistoryContentList));
                drugUsageHistory = drugUsageHistory.Where(item => !drugUsageHistoryContentList.Contains(item)).ToList();
            }
        }
        return result;
    }

    private List<DrugUsageHistoryGroupModel> SortData(int hpId, int userId, List<DrugUsageHistoryGroupModel> drugUsageGroupList)
    {
        int sortConf = _userConfRepository.GetListUserConf(hpId, userId, 1003).FirstOrDefault()?.Val ?? 0;
        foreach (var drugGrpItem in drugUsageGroupList)
        {
            drugGrpItem.SortDrugUsageHistoryContentList(sortConf);
        }
        if (sortConf == 2 || sortConf == 4)
        {
            return drugUsageGroupList.OrderByDescending(item => item.KouiKbn2).ToList();
        }
        return drugUsageGroupList.OrderBy(item => item.KouiKbn2).ToList();
    }
}
