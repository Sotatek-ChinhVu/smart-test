using Domain.Models.JsonSetting;
using System.Text.Json;
using UseCase.JsonSetting;
using UseCase.JsonSetting.GetAll;

namespace Interactor.JsonSetting;

public class GetAllJsonSettingInteractor : IGetAllJsonSettingInputPort
{
    private readonly IJsonSettingRepository _jsonSettingRepository;

    public GetAllJsonSettingInteractor(IJsonSettingRepository jsonSettingRepository)
    {
        _jsonSettingRepository = jsonSettingRepository;
    }

    public GetAllJsonSettingOutputData Handle(GetAllJsonSettingInputData input)
    {
        try
        {
            var jsons = _jsonSettingRepository.GetListFollowUserId(input.HpId, input.UserId);
            if (jsons.Count == 0)
            {
                return new GetAllJsonSettingOutputData(GetAllJsonSettingStatus.NotFound, new());
            }
            var dtos = new List<JsonSettingDto>();
            foreach (var item in jsons)
            {
                var jsonObject = JsonSerializer.Deserialize<object>(item.Value);
                dtos.Add(new JsonSettingDto(item.UserId, item.Key, jsonObject));
            }

            return new GetAllJsonSettingOutputData(GetAllJsonSettingStatus.Success, dtos);
        }
        finally
        {
            _jsonSettingRepository.ReleaseResource();
        }
    }
}
