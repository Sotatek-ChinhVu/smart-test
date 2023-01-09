using UseCase.Document;

namespace EmrCloudApi.Responses.Document.Dto;

public class ItemGroupParamDto
{
    public ItemGroupParamDto(ItemGroupParamModel model)
    {
        GroupName = model.GroupName;
        ListParams = model.ListParamModel.Select(item => new ItemParamDto(item)).ToList();
    }

    public string GroupName { get; private set; }

    public List<ItemParamDto> ListParams { get; private set; }
}
