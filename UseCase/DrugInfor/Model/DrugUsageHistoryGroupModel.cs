namespace UseCase.DrugInfor.Model;

public class DrugUsageHistoryGroupModel
{
    public DrugUsageHistoryGroupModel(int kouiKbnId, string kouiName, int kouiKbn2, List<DrugUsageHistoryContentModel> drugUsageHistoryContentList)
    {
        KouiKbnId = kouiKbnId;
        KouiName = kouiName;
        KouiKbn2 = kouiKbn2;
        DrugUsageHistoryContentList = drugUsageHistoryContentList;
    }

    public int KouiKbnId { get; private set; }

    public string KouiName { get; private set; }

    public int KouiKbn2 { get; private set; }

    public List<DrugUsageHistoryContentModel> DrugUsageHistoryContentList { get; private set; }

    public DrugUsageHistoryGroupModel SortDrugUsageHistoryContentList(int typeSort)
    {
        switch (typeSort)
        {
            case 1:
                DrugUsageHistoryContentList = DrugUsageHistoryContentList
                                              .OrderByDescending(item => item.OdrKouiKbn)
                                              .ThenByDescending(item => item.ItemCd)
                                              .ThenByDescending(item => item.Suryo)
                                              .ThenByDescending(item => item.UnitSbt)
                                              .ToList();
                break;
            case 2:
                DrugUsageHistoryContentList = DrugUsageHistoryContentList
                                              .OrderBy(item => item.OdrKouiKbn)
                                              .ThenBy(item => item.ItemName)
                                              .ThenBy(item => item.ItemCd)
                                              .ThenBy(item => item.Suryo)
                                              .ThenBy(item => item.UnitSbt)
                                              .ToList();
                break;
            case 3:
                DrugUsageHistoryContentList = DrugUsageHistoryContentList
                                              .OrderByDescending(item => item.OdrKouiKbn)
                                              .ThenByDescending(item => item.ItemName)
                                              .ThenByDescending(item => item.ItemCd)
                                              .ThenByDescending(item => item.Suryo)
                                              .ThenByDescending(item => item.UnitSbt)
                                              .ToList();
                break;
            default:
                DrugUsageHistoryContentList = DrugUsageHistoryContentList
                                              .OrderBy(item => item.OdrKouiKbn)
                                              .ThenBy(item => item.ItemCd)
                                              .ThenBy(item => item.Suryo)
                                              .ThenBy(item => item.UnitSbt)
                                              .ToList();
                break;
        }
        return this;
    }
}
