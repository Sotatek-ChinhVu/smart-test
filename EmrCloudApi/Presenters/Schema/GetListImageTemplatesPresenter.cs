using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Schema;
using UseCase.Schema.GetListImageTemplates;

namespace EmrCloudApi.Presenters.Schema;

public class GetListImageTemplatesPresenter : IGetListImageTemplatesOutputPort
{
    public Response<GetListImageTemplatesResponse> Result { get; private set; } = new Response<GetListImageTemplatesResponse>();

    public void Complete(GetListImageTemplatesOutputData output)
    {
        Result.Data = new GetListImageTemplatesResponse(output.ListFolder);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetListImageTemplatesStatus status) => status switch
    {
        GetListImageTemplatesStatus.Successed => ResponseMessage.Success,
        GetListImageTemplatesStatus.NoData => ResponseMessage.NoData,
        _ => string.Empty
    };
}