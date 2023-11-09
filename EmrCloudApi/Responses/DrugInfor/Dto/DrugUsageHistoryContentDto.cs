using UseCase.DrugInfor.Model;

namespace EmrCloudApi.Responses.DrugInfor.Dto;

public class DrugUsageHistoryContentDto
{
    public DrugUsageHistoryContentDto(DrugUsageHistoryContentModel model)
    {
        ItemName = model.ItemName;
        Quantity = model.Quantity;
        UnitName = model.UnitName;
        OdrKouiKbn = model.OdrKouiKbn;
        ListActionGraph = model.ListActionGraph;
        Tooltip = model.Tooltip;
    }

    public string ItemName { get; private set; }

    public string Quantity { get; private set; }

    public string UnitName { get; private set; }

    public int OdrKouiKbn { get; private set; }

    public List<ActionGraph> ListActionGraph { get; private set; }

    public string Tooltip { get; private set; }
}
