using Domain.Models.SystemConf;
using Domain.Models.UserConf;
using Domain.Models.VisitingListSetting;
using Helper.Common;
using Helper.Constants;
using UseCase.Reception.GetSettings;

namespace Interactor.Reception;

public class GetReceptionSettingsInteractor : IGetReceptionSettingsInputPort
{
    private readonly IUserConfRepository _userConfRepository;
    private readonly ISystemConfRepository _systemConfRepository;

    public GetReceptionSettingsInteractor(
        IUserConfRepository userConfRepository,
        ISystemConfRepository systemConfRepository)
    {
        _userConfRepository = userConfRepository;
        _systemConfRepository = systemConfRepository;
    }

    public GetReceptionSettingsOutputData Handle(GetReceptionSettingsInputData input)
    {
        var settings = GetSettings(input.UserId);
        return new GetReceptionSettingsOutputData(GetReceptionSettingsStatus.Success, settings);
    }

    private VisitingListSettingModel GetSettings(int userId)
    {
        string fontName = "";
        int fontSize = 0;
        int autoRefresh = 0;
        int mouseWheel = 0;
        int kanFocus = 0;
        int selectToDoSetting = 0;
        var userConfigs = _userConfRepository.GetList(userId, UserConfCommon.GroupCodes.Font, UserConfCommon.GroupCodes.SelectTodoSetting);
        foreach (var config in userConfigs)
        {
            switch (config.GrpCd)
            {
                case UserConfCommon.GroupCodes.Font:
                    fontName = config.Param;
                    fontSize = config.Val;
                    break;
                case UserConfCommon.GroupCodes.AutoRefresh:
                    autoRefresh = config.Val;
                    break;
                case UserConfCommon.GroupCodes.MouseWheel:
                    mouseWheel = config.Val;
                    break;
                case UserConfCommon.GroupCodes.KanFocus:
                    kanFocus = config.Val;
                    break;
                case UserConfCommon.GroupCodes.SelectTodoSetting:
                    selectToDoSetting = config.Val;
                    break;
                default:
                    break;
            }
        }

        var systemConfigs = _systemConfRepository.GetList(SystemConfGroupCodes.ReceptionTimeColor, SystemConfGroupCodes.ReceptionStatusColor);

        var receptionTimeColorConfigs = systemConfigs
                .Where(c => c.GrpCd == SystemConfGroupCodes.ReceptionTimeColor)
                .Select(c => new ReceptionTimeColorConfig(c.GrpEdaNo, c.Param))
                .OrderBy(c => c.Duration)
                .ThenBy(c => c.Color)
                .ToList();

        var receptionStatusColorConfigs = systemConfigs
                .Where(c => c.GrpCd == SystemConfGroupCodes.ReceptionStatusColor)
                .Select(c => new ReceptionStatusColorConfig(c.GrpEdaNo, c.Param))
                .ToList();

        receptionStatusColorConfigs = StandardizeReceptionStatusColorConfigs(receptionStatusColorConfigs);

        return new VisitingListSettingModel(receptionTimeColorConfigs, receptionStatusColorConfigs);
    }

    private List<ReceptionStatusColorConfig> StandardizeReceptionStatusColorConfigs(List<ReceptionStatusColorConfig> configs)
    {
        var receptionStatuses = RaiinState.ReceptionStatusToText.Select(pair => pair.Key).ToList();
        var matchingStatusVsConfigQuery =
            from status in receptionStatuses
            join config in configs on status equals config.Status into colorConfigs
            from colorConfig in colorConfigs.DefaultIfEmpty() // Left join with statuses
            orderby status
            select colorConfig ?? new ReceptionStatusColorConfig(status); // Set the default value for the missing config

        return matchingStatusVsConfigQuery.ToList();
    }
}
