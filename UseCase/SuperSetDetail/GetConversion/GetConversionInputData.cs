using UseCase.Core.Sync.Core;

namespace UseCase.SuperSetDetail.GetConversion;

public class GetConversionInputData : IInputData<GetConversionOutputData>
{
    public GetConversionInputData(int hpId, string itemCd, int sinDate, string itemName, double quantity, string unitName)
    {
        HpId = hpId;
        ItemCd = itemCd;
        SinDate = sinDate;
        ItemName = itemName;
        Quantity = quantity;
        UnitName = unitName;
    }

    public int HpId { get; private set; }

    public string ItemCd { get; private set; }

    public int SinDate { get; private set; }

    public string ItemName { get; private set; }

    public double Quantity { get; private set; }

    public string UnitName { get; private set; }
}
