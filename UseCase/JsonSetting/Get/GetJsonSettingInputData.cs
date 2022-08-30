using UseCase.Core.Sync.Core;

namespace UseCase.JsonSetting.Get;

public class GetJsonSettingInputData : IInputData<GetJsonSettingOutputData>
{
    public GetJsonSettingInputData(int userId, string key)
    {
        UserId = userId;
        Key = key;
    }

    public int UserId { get; private set; }
    public string Key { get; private set; }
}
