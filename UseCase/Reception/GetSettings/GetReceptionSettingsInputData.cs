using UseCase.Core.Sync.Core;

namespace UseCase.Reception.GetSettings;

public class GetReceptionSettingsInputData : IInputData<GetReceptionSettingsOutputData>
{
    public GetReceptionSettingsInputData(int userId, int hpId)
    {
        UserId = userId;
        HpId = hpId;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }
}
