using Domain.Models.DrugInfor;
using Helper.Extension;
using UseCase.DrugInfor.GetContentDrugUsageHistory;
using UseCase.DrugInfor.Model;

namespace Interactor.DrugInfor;

public class GetContentDrugUsageHistoryInteractor : IGetContentDrugUsageHistoryInputPort
{
    private readonly IDrugInforRepository _drugInforRepository;

    public GetContentDrugUsageHistoryInteractor(IDrugInforRepository drugInforRepository)
    {
        _drugInforRepository = drugInforRepository;
    }
    public GetContentDrugUsageHistoryOutputData Handle(GetContentDrugUsageHistoryInputData inputData)
    {
        try
        {
            List<DrugUsageHistoryContentModel> drugUsageHistory = GetData(inputData.HpId, inputData.PtId, inputData.GrpCd, inputData.StartDate, inputData.EndDate);
            return new GetContentDrugUsageHistoryOutputData(drugUsageHistory, GetContentDrugUsageHistoryStatus.Successed);
        }
        finally
        {
            _drugInforRepository.ReleaseResource();
        }
    }

    private List<DrugUsageHistoryContentModel> GetData(int hpId, long ptId, int grpId, int startDate, int endDate, int mode = 0)
    {
        List<DrugUsageHistoryContentModel> drugUsageHistory = new();
        var drugUsageHistoryList = _drugInforRepository.GetDrugUsageHistoryList(hpId, ptId);
        var sinrekiFilter = _drugInforRepository.GetSinrekiFilterMst(hpId, grpId);
        if (mode == 0)
        {
            drugUsageHistoryList = drugUsageHistoryList.Where(item => item.SinDate >= startDate && item.SinDate <= endDate
                                                                      || item.KouiKbn == 21 && item.SinDate < startDate && item.EndDate >= startDate)
                                                       .ToList();
        }
        else
        {
            drugUsageHistoryList = drugUsageHistoryList.Where(item => item.SinDate >= startDate && item.SinDate <= endDate).ToList();
        }
        if (grpId > 0)
        {
            drugUsageHistoryList = FilterData(drugUsageHistoryList, sinrekiFilter.SinrekiFilterMstDetailList);
            drugUsageHistoryList = FilterData(drugUsageHistoryList, sinrekiFilter.SinrekiFilterMstKouiList);
        }
        var drugUsageHistoryDistinctList = GetDataDistinct(drugUsageHistoryList);

        return drugUsageHistory;
    }

    private List<DrugUsageHistoryModel> FilterData(List<DrugUsageHistoryModel> listDrugUsageHistory, List<SinrekiFilterMstKouiModel> sinrekiFilterMstKouiList)
    {
        return listDrugUsageHistory;
        var kouiKbnFilter = GetListKouiKbnFilter(sinrekiFilterMstKouiList);

        return listDrugUsageHistory.Where(item => kouiKbnFilter.Contains(item.KouiKbn)).ToList();
    }

    private List<DrugUsageHistoryModel> FilterData(List<DrugUsageHistoryModel> listDrugUsageHistory, List<SinrekiFilterMstDetailModel> sinrekiFilterMstDetailList)
    {
        var itemCdList = sinrekiFilterMstDetailList.Where(item => !item.IsExclude).Select(item => item.ItemCd).Distinct().ToList();
        listDrugUsageHistory = listDrugUsageHistory.Where(item => itemCdList.Contains(item.ItemCd)).ToList();
        return listDrugUsageHistory;
    }

    private List<int> GetListKouiKbnFilter(List<SinrekiFilterMstKouiModel> sinrekiFilterMstKouiList)
    {
        List<int> listKouiKbn = new();

        //if (DrugUsageHistoryConf.KouiSyosai == true)
        //    listKouiKbn.AddRange(new List<int> { 9, 10 });
        //if (DrugUsageHistoryConf.KouiIKan == true)
        //    listKouiKbn.AddRange(new List<int> { 11, 12, 13 });
        //if (DrugUsageHistoryConf.KouiZaitaku == true)
        //    listKouiKbn.Add(14);

        //if (DrugUsageHistoryConf.KouiNai == true)
        //    listKouiKbn.Add(21);
        //if (DrugUsageHistoryConf.KouiTon == true)
        //    listKouiKbn.Add(22);
        //if (DrugUsageHistoryConf.KouiGai == true)
        //    listKouiKbn.Add(23);

        //if (DrugUsageHistoryConf.KouiChusha == true)
        //    listKouiKbn.AddRange(new List<int> { 31, 32, 33, 34 });
        //if (DrugUsageHistoryConf.KouiJikocyu == true)
        //    listKouiKbn.Add(28);

        //if (DrugUsageHistoryConf.KouiSyochi == true)
        //    listKouiKbn.Add(40);
        //if (DrugUsageHistoryConf.KouiSyujyutu == true)
        //    listKouiKbn.Add(50);
        //if (DrugUsageHistoryConf.KouiKensa == true)
        //    listKouiKbn.AddRange(new List<int> { 60, 61, 62, 63, 64 });

        //if (DrugUsageHistoryConf.KouiGazo == true)
        //    listKouiKbn.Add(70);
        //if (DrugUsageHistoryConf.KouiSonota == true)
        //    listKouiKbn.AddRange(new List<int> { 80, 81, 82, 84, 100, 101 });
        //if (DrugUsageHistoryConf.KouiJihi == true)
        //    listKouiKbn.AddRange(new List<int> { 95, 96 });

        return listKouiKbn;
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
            if (!result.Any(item => item.ItemCd == drugUsage.ItemCd && item.KouiKbn == drugUsage.KouiKbn &&
                item.Quantity.AsString() == drugUsage.Quantity.AsString()))
            {
                result.Add(drugUsage);
            }
        }

        return result;
    }
}
