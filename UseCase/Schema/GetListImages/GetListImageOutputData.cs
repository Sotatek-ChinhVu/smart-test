using UseCase.Core.Sync.Core;

namespace UseCase.Schema.GetListImages;

public class GetListImageOutputData : IOutputData
{
    public GetListImageOutputData(List<GetListImageOutputItem> listFolder, GetListImageStatus status)
    {
        ListFolder = listFolder;
        Status = status;
    }

    public List<GetListImageOutputItem> ListFolder { get; private set; }
    public GetListImageStatus Status { get; private set; }
}
