using UseCase.Document;

namespace EmrCloudApi.Responses.Document;

public class ItemParamDto
{
    public ItemParamDto(ItemDisplayParamModel model)
    {
        Parameter = model.Parameter;
        Value = model.Value;
    }

    public string Parameter { get; private set; }

    public string Value { get; private set; }
}
