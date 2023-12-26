using UseCase.Core.Sync.Core;

namespace UseCase.JsonSetting.GetAll;

public class GetAllJsonSettingOutputData : IOutputData
{
    public GetAllJsonSettingOutputData(GetAllJsonSettingStatus status, List<JsonSettingDto> settings)
    {
        Status = status;
        Settings = settings;
    }

    public GetAllJsonSettingStatus Status { get; private set; }
    public List<JsonSettingDto> Settings { get; private set; }
}
