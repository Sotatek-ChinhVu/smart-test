using UseCase.Document;

namespace EmrCloudApi.Responses.Document;

public class ItemParamDto
{
    public ItemParamDto(ItemDisplayParamModel model)
    {
        GroupName = model.GroupName;
        Parameter = model.Parameter;
        Value = model.Value;
    }

    public string GroupName { get; private set; }

    public string Parameter { get; private set; }

    public string Value { get; private set; }
}
