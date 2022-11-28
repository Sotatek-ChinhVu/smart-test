using UseCase.Schema.GetListImageTemplates;

namespace EmrCloudApi.Responses.Schema;

public class GetListImageTemplatesResponse
{
    public GetListImageTemplatesResponse(List<GetListImageTemplatesOutputItem> data)
    {
        Data = data;
    }

    public List<GetListImageTemplatesOutputItem> Data { get; private set; }
}
