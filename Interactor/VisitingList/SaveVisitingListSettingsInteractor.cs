using Domain.Models.SystemConf;
using Domain.Models.VisitingListSetting;
using Helper.Common;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.VisitingList.SaveSettings;

namespace Interactor.VisitingList;

public class SaveVisitingListSettingsInteractor : ISaveVisitingListSettingsInputPort
{
    private readonly IVisitingListSettingRepository _visitingListSettingRepository;
    private readonly ILoggingHandler _loggingHandler;

    public SaveVisitingListSettingsInteractor(ITenantProvider tenantProvider, IVisitingListSettingRepository visitingListSettingRepository)
    {
        _visitingListSettingRepository = visitingListSettingRepository;
        _loggingHandler = new LoggingHandler(tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
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
        catch (Exception ex)
        {
            _loggingHandler.WriteLogExceptionAsync(ex);
            throw;
        }
        finally
        {
            _visitingListSettingRepository.ReleaseResource();
            _loggingHandler.Dispose();
        }
    }
}
