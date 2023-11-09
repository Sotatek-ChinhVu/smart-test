namespace UseCase.DrugInfor.Model;

public class DrugUsageHistoryGroupModel
{
    public DrugUsageHistoryGroupModel(int kouiKbnId, string kouiName, List<DrugUsageHistoryContentModel> drugUsageHistoryContentList)
    {
        KouiKbnId = kouiKbnId;
        KouiName = kouiName;
        DrugUsageHistoryContentList = drugUsageHistoryContentList;
    }

    public int KouiKbnId { get; private set; }

    public string KouiName { get; private set; }

    public List<DrugUsageHistoryContentModel> DrugUsageHistoryContentList { get; private set; }
}
