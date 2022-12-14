using UseCase.Core.Sync.Core;

namespace UseCase.Document.GetListParamTemplate;

public class GetListParamTemplateOutputData : IOutputData
{
    public GetListParamTemplateOutputData(GetListParamTemplateStatus status)
    {
        Status = status;
        ListParams = new();
    }

    public GetListParamTemplateOutputData(List<ItemDisplayParamModel> listParams, GetListParamTemplateStatus status)
    {
        ListParams = listParams;
        Status = status;
    }

    public List<ItemDisplayParamModel> ListParams { get; private set; }

    public GetListParamTemplateStatus Status { get; private set; }
}
