using Domain.Models.SystemConf;
using Domain.Models.VisitingListSetting;
using Helper.Common;
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
        try
        {
            var timeColorSystemConfs = input.Settings.ReceptionTimeColorConfigs
            .Select(c => new SystemConfModel(SystemConfGroupCodes.ReceptionTimeColor, c.Duration, 0, c.Color, string.Empty));
            var statusColorSystemConfs = input.Settings.ReceptionStatusColorConfigs
                .Select(c => new SystemConfModel(SystemConfGroupCodes.ReceptionStatusColor, c.Status, 0, c.Color, string.Empty));
            var systemConfs = timeColorSystemConfs.Concat(statusColorSystemConfs).ToList();

            _visitingListSettingRepository.Save(systemConfs, input.HpId, input.UserId);
            return new SaveVisitingListSettingsOutputData(SaveVisitingListSettingsStatus.Success);
        }
        finally
        {
            _visitingListSettingRepository.ReleaseResource();
        }
    }
}
