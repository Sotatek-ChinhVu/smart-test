using Domain.Models.JsonSetting;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using System.Text.Json;
using UseCase.JsonSetting.Upsert;

namespace Interactor.JsonSetting;

public class UpsertJsonSettingInteractor : IUpsertJsonSettingInputPort
{
    private readonly IJsonSettingRepository _jsonSettingRepository;
    private readonly ILoggingHandler _loggingHandler;
    private readonly ITenantProvider _tenantProvider;

    public UpsertJsonSettingInteractor(ITenantProvider tenantProvider, IJsonSettingRepository jsonSettingRepository)
    {
        _jsonSettingRepository = jsonSettingRepository;
        _tenantProvider = tenantProvider;
        _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
    }

    public UpsertJsonSettingOutputData Handle(UpsertJsonSettingInputData input)
    {
        try
        {
            string jsonString = JsonSerializer.Serialize(input.Setting.Value);
            var setting = new JsonSettingModel(input.Setting.UserId, input.Setting.Key, jsonString);
            _jsonSettingRepository.Upsert(setting);

            return new UpsertJsonSettingOutputData(UpsertJsonSettingStatus.Success);
        }
        catch (Exception ex)
        {
            _loggingHandler.WriteLogExceptionAsync(ex);
            throw;
        }
        finally
        {
            _jsonSettingRepository.ReleaseResource();
            _loggingHandler.Dispose();
        }
    }
}
