using Domain.Models.JsonSetting;
using UseCase.Core.Sync.Core;

namespace UseCase.JsonSetting.Upsert;

public class UpsertJsonSettingOutputData : IOutputData
{
    public UpsertJsonSettingOutputData(UpsertJsonSettingStatus status)
    {
        Status = status;
    }

    public UpsertJsonSettingStatus Status { get; private set; }
}
