using EmrCloudApi.Responses.Document.Dto;
using UseCase.Document;

namespace EmrCloudApi.Responses.Document;

public class GetListParamTemplateResponse
{
    public GetListParamTemplateResponse(List<ItemGroupParamModel> data)
    {
        Data = data.Select(item => new ItemGroupParamDto(item)).ToList();
    }

    public List<ItemGroupParamDto> Data { get; private set; }
}
