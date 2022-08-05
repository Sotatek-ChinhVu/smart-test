using Domain.Models.Reception;
using UseCase.Core.Sync.Core;

namespace UseCase.Reception.GetSettings;

public class GetReceptionSettingsOutputData : IOutputData
{
    public GetReceptionSettingsOutputData(GetReceptionSettingsStatus status, ReceptionSettings settings)
    {
        Status = status;
        Settings = settings;
    }

    public GetReceptionSettingsStatus Status { get; private set; }
    public ReceptionSettings Settings { get; private set; }
}
