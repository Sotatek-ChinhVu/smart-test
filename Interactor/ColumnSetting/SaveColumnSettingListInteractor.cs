using Domain.Models.ColumnSetting;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.ColumnSetting.SaveList;

namespace Interactor.ColumnSetting;

public class SaveColumnSettingListInteractor : ISaveColumnSettingListInputPort
{
    private readonly IColumnSettingRepository _columnSettingRepository;
    private readonly ILoggingHandler _loggingHandler;

    public SaveColumnSettingListInteractor(ITenantProvider tenantProvider, IColumnSettingRepository columnSettingRepository)
    {
        _columnSettingRepository = columnSettingRepository;
        _loggingHandler = new LoggingHandler(tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
    }

    public SaveColumnSettingListOutputData Handle(SaveColumnSettingListInputData input)
    {
        try
        {
            bool success = _columnSettingRepository.SaveList(input.Settings);
            var status = success ? SaveColumnSettingListStatus.Success : SaveColumnSettingListStatus.Failed;
            return new SaveColumnSettingListOutputData(status);
        }
        catch (Exception ex)
        {
            _loggingHandler.WriteLogExceptionAsync(ex);
            throw;
        }
        finally
        {
            _columnSettingRepository.ReleaseResource();
            _loggingHandler.Dispose();
        }
    }
}
