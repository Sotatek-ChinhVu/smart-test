using UseCase.Core.Sync.Core;

namespace UseCase.Reception.GetSettings;

public class GetReceptionSettingsInputData : IInputData<GetReceptionSettingsOutputData>
{
    public GetReceptionSettingsInputData(int userId)
    {
        UserId = userId;
    }

    public int UserId { get; private set; }
}
