using Domain.Models.JsonSetting;
using System.Text.Json;
using UseCase.JsonSetting.Upsert;

namespace Interactor.JsonSetting;

public class UpsertJsonSettingInteractor : IUpsertJsonSettingInputPort
{
    private readonly IJsonSettingRepository _jsonSettingRepository;

    public UpsertJsonSettingInteractor(IJsonSettingRepository jsonSettingRepository)
    {
        _jsonSettingRepository = jsonSettingRepository;
    }

    public UpsertJsonSettingOutputData Handle(UpsertJsonSettingInputData input)
    {
        string jsonString = JsonSerializer.Serialize(input.Setting.Value);
        var setting = new JsonSettingModel(input.Setting.UserId, input.Setting.Key, jsonString);
        _jsonSettingRepository.Upsert(setting);

        return new UpsertJsonSettingOutputData(UpsertJsonSettingStatus.Success);
    }
}
