using UseCase.Core.Sync.Core;

namespace UseCase.Reception.GetVisitingColumnSettings;

public class GetVisitingColumnSettingsInputData : IInputData<GetVisitingColumnSettingsOutputData>
{
    public GetVisitingColumnSettingsInputData(int userId)
    {
        UserId = userId;
    }

    public int UserId { get; private set; }
}
