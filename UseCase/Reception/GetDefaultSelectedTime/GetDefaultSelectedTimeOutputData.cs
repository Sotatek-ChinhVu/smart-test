using Domain.Models.TimeZone;
using UseCase.Core.Sync.Core;

namespace UseCase.Reception.GetDefaultSelectedTime;

public class GetDefaultSelectedTimeOutputData : IOutputData
{
    public GetDefaultSelectedTimeOutputData(GetDefaultSelectedTimeStatus status)
    {
        Status = status;
        Data = new DefaultSelectedTimeModel();
    }

    public GetDefaultSelectedTimeOutputData(GetDefaultSelectedTimeStatus status, DefaultSelectedTimeModel data)
    {
        Status = status;
        Data = data;
    }

    public GetDefaultSelectedTimeStatus Status { get; private set; }

    public DefaultSelectedTimeModel Data { get; private set; }
}
