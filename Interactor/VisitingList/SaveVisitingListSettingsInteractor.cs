using Domain.Models.SystemConf;
using Domain.Models.UserConfig;
using Domain.Models.VisitingListSetting;
using Helper.Common;
using Helper.Constants;
using UseCase.VisitingList.SaveSettings;

namespace Interactor.VisitingList;

public class SaveVisitingListSettingsInteractor : ISaveVisitingListSettingsInputPort
{
    private readonly IVisitingListSettingRepository _visitingListSettingRepository;

    public SaveVisitingListSettingsInteractor(IVisitingListSettingRepository visitingListSettingRepository)
    {
        _visitingListSettingRepository = visitingListSettingRepository;
    }

    public SaveVisitingListSettingsOutputData Handle(SaveVisitingListSettingsInputData input)
    {
        var userConfs = new List<UserConfigModel>
        {
            CreateUserConfModel(input.UserId, UserConfCommon.GroupCodes.Font, input.Settings.FontSize, input.Settings.FontName),
            CreateUserConfModel(input.UserId, UserConfCommon.GroupCodes.AutoRefresh, input.Settings.AutoRefresh, string.Empty),
            CreateUserConfModel(input.UserId, UserConfCommon.GroupCodes.MouseWheel, input.Settings.MouseWheel, string.Empty),
            CreateUserConfModel(input.UserId, UserConfCommon.GroupCodes.KanFocus, input.Settings.KanFocus, string.Empty),
            CreateUserConfModel(input.UserId, UserConfCommon.GroupCodes.SelectTodoSetting, input.Settings.SelectTodoSetting, string.Empty)
        };

        var timeColorSystemConfs = input.Settings.ReceptionTimeColorConfigs
            .Select(c => new SystemConfModel(SystemConfGroupCodes.ReceptionTimeColor, c.Duration, 0, c.Color, string.Empty));
        var statusColorSystemConfs = input.Settings.ReceptionStatusColorConfigs
            .Select(c => new SystemConfModel(SystemConfGroupCodes.ReceptionStatusColor, c.Status, 0, c.Color, string.Empty));
        var systemConfs = timeColorSystemConfs.Concat(statusColorSystemConfs).ToList();

        _visitingListSettingRepository.Save(userConfs, systemConfs);
        return new SaveVisitingListSettingsOutputData(SaveVisitingListSettingsStatus.Success);
    }

    private UserConfigModel CreateUserConfModel(int userId, int grpCd, int val, string param)
    {
        return new UserConfigModel(TempIdentity.HpId, userId, grpCd, 0, 0, val, param);
    }
}
