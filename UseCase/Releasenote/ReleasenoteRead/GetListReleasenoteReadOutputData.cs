using UseCase.Core.Sync.Core;

namespace UseCase.ReleasenoteRead;

public class GetListReleasenoteReadOutputData : IOutputData
{
    public GetListReleasenoteReadOutputData(GetListReleasenoteReadStatus status, List<string> version)
    {
        Status = status;
        Version = version;
    }

    public GetListReleasenoteReadStatus Status { get; private set; }

    public List<string> Version { get; private set; }
}
