using UseCase.Core.Sync.Core;

namespace UseCase.Reception.UpdateTimeZoneDayInf;

public class UpdateTimeZoneDayInfOutputData : IOutputData
{
    public UpdateTimeZoneDayInfOutputData(UpdateTimeZoneDayInfStatus status)
    {
        Status = status;
    }

    public UpdateTimeZoneDayInfStatus Status { get; private set; }
}
