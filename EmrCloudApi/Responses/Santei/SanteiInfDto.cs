using UseCase.Santei.GetListSanteiInf;

namespace EmrCloudApi.Responses.Santei;

public class SanteiInfDto
{
    public SanteiInfDto(SanteiInfOutputItem outputItem)
    {
        Id = outputItem.Id;
        PtId = outputItem.PtId;
        ItemCd = outputItem.ItemCd;
        FixedItem = outputItem.FixedItem;
        IsOutDate = outputItem.IsOutDate;
        ItemName = outputItem.ItemName;
        IsSettedItemCd = outputItem.IsSettedItemCd;
        LastOdrDateDisplay = outputItem.LastOdrDateDisplay;
        KisanDateDisplay = outputItem.KisanDateDisplay;
        DayCountDisplay = outputItem.DayCountDisplay;
        SanteiItemCountDisplay = outputItem.SanteiItemCountDisplay;
        SanteiItemSumDisplay = outputItem.SanteiItemSumDisplay;
        AlertDays = outputItem.AlertDays;
        AlertTerm = outputItem.AlertTerm;
        IsKensaItem = outputItem.IsKensaItem;
        ListSanteInfDetails = outputItem.ListSanteInfDetails.Select(item => new SanteiInfDetailDto(item)).ToList();
    }

    public long Id { get; private set; }

    public long PtId { get; private set; }

    public string ItemCd { get; private set; }

    public string FixedItem { get; private set; }

    public bool IsOutDate { get; private set; }

    public string ItemName { get; private set; }

    public bool IsSettedItemCd { get; private set; }

    public string LastOdrDateDisplay { get; private set; }

    public string KisanDateDisplay { get; private set; }

    public string DayCountDisplay { get; private set; }

    public string SanteiItemCountDisplay { get; private set; }

    public string SanteiItemSumDisplay { get; private set; }

    public int AlertDays { get; private set; }

    public int AlertTerm { get; private set; }

    public bool IsKensaItem { get; private set; }

    public List<SanteiInfDetailDto> ListSanteInfDetails { get; private set; }
}
