using UseCase.Core.Sync.Core;

namespace UseCase.Online.UpdatePtInfOnlineQualify;

public class UpdatePtInfOnlineQualifyOutputData : IOutputData
{
    public UpdatePtInfOnlineQualifyOutputData(UpdatePtInfOnlineQualifyStatus status)
    {
        Status = status;
    }

    public UpdatePtInfOnlineQualifyStatus Status { get; private set; }
}
