using UseCase.Core.Sync.Core;

namespace UseCase.JsonSetting.GetAll;

public class GetAllJsonSettingInputData : IInputData<GetAllJsonSettingOutputData>
{
    public GetAllJsonSettingInputData(int userId)
    {
        UserId = userId;
    }

    public int UserId { get; private set; }
}
