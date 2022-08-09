using Domain.Models.Reception;
using Domain.Models.SystemConf;
using Domain.Models.UserConf;
using Domain.Models.VisitingListSetting;
using Helper.Common;
using UseCase.Reception.GetSettings;

namespace Interactor.Reception;

public class GetReceptionSettingsInteractor : IGetReceptionSettingsInputPort
{
    private readonly IVisitingListSettingRepository _visitingListSettingRepository;

    public GetReceptionSettingsInteractor(
        IVisitingListSettingRepository visitingListSettingRepository)
    {
        _visitingListSettingRepository = visitingListSettingRepository;
    }

    public GetReceptionSettingsOutputData Handle(GetReceptionSettingsInputData input)
    {
        var visitingListSettingModel = _visitingListSettingRepository.Get(input.UserId);
        return new GetReceptionSettingsOutputData(GetReceptionSettingsStatus.Success, visitingListSettingModel);
    }
}
