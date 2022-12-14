using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Document;
using UseCase.Document.GetListParamTemplate;

namespace EmrCloudApi.Presenters.Document;

public class GetListParamTemplatePresenter : IGetListParamTemplateOutputPort
{
    public Response<GetListParamTemplateResponse> Result { get; private set; } = new();

    public void Complete(GetListParamTemplateOutputData output)
    {
        Result.Data = new GetListParamTemplateResponse(output.ListParams.Select(item => new ItemParamDto(item)).ToList());
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetListParamTemplateStatus status) => status switch
    {
        GetListParamTemplateStatus.Successed => ResponseMessage.Success,
        GetListParamTemplateStatus.Failed => ResponseMessage.Failed,
        _ => string.Empty
    };
}

