namespace EmrCloudApi.Responses.Receipt.Dto;

public class ReceByomeiCheckingDto
{
    public ReceByomeiCheckingDto(string displayItemName, List<ByomeiCheckingDto> listCheckingItem)
    {
        DisplayItemName = displayItemName;
        ListCheckingItem = listCheckingItem;
    }

    public string DisplayItemName { get; private set; }

    public List<ByomeiCheckingDto> ListCheckingItem { get; private set; }
}
