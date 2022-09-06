using UseCase.Core.Sync.Core;

namespace UseCase.Schema.GetListImageTemplates;

public class GetListImageTemplatesOutputData : IOutputData
{
    public GetListImageTemplatesOutputData(List<GetListImageTemplatesOutputItem> listFolder, GetListImageTemplatesStatus status)
    {
        ListFolder = listFolder;
        Status = status;
    }

    public List<GetListImageTemplatesOutputItem> ListFolder { get; private set; }
    public GetListImageTemplatesStatus Status { get; private set; }
}
