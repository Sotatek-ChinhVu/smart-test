using UseCase.Core.Sync.Core;

namespace UseCase.JsonSetting.GetAll;

public class GetAllJsonSettingInputData : IInputData<GetAllJsonSettingOutputData>
{
    public GetAllJsonSettingInputData(int hpId, int userId)
    {
        HpId = hpId;
        UserId = userId;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }
}
