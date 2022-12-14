namespace EmrCloudApi.Responses.Document;

public class GetListParamTemplateResponse
{
    public GetListParamTemplateResponse(List<ItemParamDto> data)
    {
        Data = data;
    }

    public List<ItemParamDto> Data { get; private set; }
}
