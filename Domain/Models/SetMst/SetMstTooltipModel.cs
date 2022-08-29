namespace Domain.Models.SetMst;

public class SetMstTooltipModel
{
    public SetMstTooltipModel(List<string> listKarteNames, List<OrderTooltipModel> listOrders, List<string> listByomeis)
    {
        ListKarteNames = listKarteNames;
        ListOrders = listOrders;
        ListByomeis = listByomeis;
    }

    public List<string> ListKarteNames { get; private set; }
    public List<OrderTooltipModel> ListOrders { get; private set; }
    public List<string> ListByomeis { get; private set; }
}