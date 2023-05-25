namespace Domain.Models.SetMst;

public class SetMstTooltipModel
{
    public SetMstTooltipModel(List<string> listKarteNames, Dictionary<long, List<OrderTooltipModel>> listOrders, List<string> listByomeis)
    {
        ListKarteNames = listKarteNames;
        DictionaryOrders = listOrders;
        ListByomeis = listByomeis;
    }

    public SetMstTooltipModel()
    {
        ListKarteNames = new List<string>();
        DictionaryOrders = new();
        ListByomeis = new List<string>();
    }

    public List<string> ListKarteNames { get; private set; }
    public Dictionary<long, List<OrderTooltipModel>> DictionaryOrders { get; private set; }
    public List<string> ListByomeis { get; private set; }
}