using Domain.Models.Reception;
using Domain.Models.SystemConf;
using Domain.Models.UserConf;
using Helper.Common;
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

    private ReceptionSettings GetSettings(int userId)
    {
        var userConfigs = _userConfRepository.GetList(userId, UserConfCommon.GroupCodes.Font, UserConfCommon.GroupCodes.SelectTodoSetting);
        var systemConfigs = _systemConfRepository.GetList(SystemConfGroupCodes.ReceptionTimeColor, SystemConfGroupCodes.ReceptionStatusColor);
        return new ReceptionSettings(userConfigs, systemConfigs);
    }
}
