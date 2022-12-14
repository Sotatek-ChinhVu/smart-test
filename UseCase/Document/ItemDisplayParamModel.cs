namespace UseCase.Document;

public class ItemDisplayParamModel
{
    public ItemDisplayParamModel(string parameter, string value)
    {
        Parameter = parameter;
        Value = value;
    }

    public ItemDisplayParamModel(string parameter)
    {
        Parameter = parameter;
        Value = string.Empty;
    }

    public string Parameter { get; private set; }

    public string Value { get; private set; }
}
