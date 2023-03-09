using Domain.Models.OrdInfDetails;

namespace Domain.Models.Receipt.Recalculation;

public class BuiErrorModel
{
    public OrdInfDetailModel OdrInfDetail { get; }
    public int OdrKouiKbn { get; }
    public int SinDate { get; }
    public string ItemName { get; }
    public List<string> Errors { get; }

    public BuiErrorModel(OrdInfDetailModel odrInfDetail, int odrKouiKbn, int sinDate, string itemName)
    {
        OdrInfDetail = odrInfDetail;
        OdrKouiKbn = odrKouiKbn;
        SinDate = sinDate;
        ItemName = itemName;
        Errors = new List<string>();
    }
}
