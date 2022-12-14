using UseCase.Core.Sync.Core;

namespace UseCase.Document.GetListParamTemplate;

public class GetListParamTemplateOutputData : IOutputData
{
    public GetListParamTemplateOutputData(GetListParamTemplateStatus status)
    {
        Status = status;
        ListGroups = new();
    }

    public GetListParamTemplateOutputData(List<ItemGroupParamModel> listGroups, GetListParamTemplateStatus status)
    {
        ListGroups = listGroups;
        Status = status;
    }

    public List<ItemGroupParamModel> ListGroups { get; private set; }

    public GetListParamTemplateStatus Status { get; private set; }
}
