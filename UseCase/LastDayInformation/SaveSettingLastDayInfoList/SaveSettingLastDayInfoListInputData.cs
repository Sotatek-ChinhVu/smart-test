using Domain.Models.TodayOdr;
using UseCase.Core.Sync.Core;

namespace UseCase.LastDayInformation.SaveSettingLastDayInfoList;

public class SaveSettingLastDayInfoListInputData : IInputData<SaveSettingLastDayInfoListOutputData>
{
    public SaveSettingLastDayInfoListInputData(int hpId, int userId, List<OdrDateInfModel> odrDateInfModels)
    {
        HpId = hpId;
        UserId = userId;
        OdrDateInfModels = odrDateInfModels;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public List<OdrDateInfModel> OdrDateInfModels { get; private set; }
}
