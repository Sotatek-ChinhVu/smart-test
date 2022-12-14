namespace UseCase.Document;

public class ItemDisplayParamModel
{
    public ItemDisplayParamModel(string groupName, string parameter, string value)
    {
        GroupName = groupName;
        Parameter = parameter;
        Value = value;
    }

    public ItemDisplayParamModel(string groupName, string parameter)
    {
        GroupName = groupName;
        Parameter = parameter;
        Value = string.Empty;
    }

    public string GroupName { get; private set; }

    public string Parameter { get; private set; }

    public string Value { get; private set; }
}
