namespace UseCase.Document;

public class ItemGroupParamModel
{
    public ItemGroupParamModel(string groupName, List<ItemDisplayParamModel> listParamModel)
    {
        GroupName = groupName;
        ListParamModel = listParamModel;
    }

    public string GroupName { get; private set; }

    public List<ItemDisplayParamModel> ListParamModel { get; private set; }
}
