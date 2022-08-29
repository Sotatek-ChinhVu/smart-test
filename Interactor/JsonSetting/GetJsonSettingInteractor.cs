using Domain.Models.JsonSetting;
using System.Text.Json;
using UseCase.JsonSetting;
using UseCase.JsonSetting.Get;

namespace Interactor.JsonSetting;

public class GetJsonSettingInteractor : IGetJsonSettingInputPort
{
    private readonly IJsonSettingRepository _jsonSettingRepository;

    public GetJsonSettingInteractor(IJsonSettingRepository jsonSettingRepository)
    {
        _jsonSettingRepository = jsonSettingRepository;
    }

    public GetJsonSettingOutputData Handle(GetJsonSettingInputData input)
    {
        var model = _jsonSettingRepository.Get(input.UserId, input.Key);
        if (model is null)
        {
            return new GetJsonSettingOutputData(GetJsonSettingStatus.NotFound, null);
        }

        var jsonObject = JsonSerializer.Deserialize<object>(model.Value);
        var dto = new JsonSettingDto(model.UserId, model.Key, jsonObject);
        return new GetJsonSettingOutputData(GetJsonSettingStatus.Success, dto);
    }
}
