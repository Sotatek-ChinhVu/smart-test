using UseCase.Core.Sync.Core;

namespace UseCase.JsonSetting.Get;

public class GetJsonSettingInputData : IInputData<GetJsonSettingOutputData>
{
    public GetJsonSettingInputData(int hpId, int userId, string key)
    {
        HpId = hpId;
        UserId = userId;
        Key = key;
    }

    public int HpId { get; private set; }
    public int UserId { get; private set; }
    public string Key { get; private set; }
}
