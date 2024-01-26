using EmrCloudApi.Responses.Yousiki;
using UseCase.Yousiki.DeleteYousikiInf;
using EmrCloudApi.Responses;
using EmrCloudApi.Constants;

namespace EmrCloudApi.Presenters.Yousiki;

public class DeleteYousikiInfPresenter : IDeleteYousikiInfOutputPort
{
    public Response<DeleteYousikiInfResponse> Result { get; private set; } = new();
    public void Complete(DeleteYousikiInfOutputData outputData)
    {
        Result.Data = new DeleteYousikiInfResponse(outputData.Status == DeleteYousikiInfStatus.Successed);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(DeleteYousikiInfStatus status) => status switch
    {
        DeleteYousikiInfStatus.Successed => ResponseMessage.Success,
        DeleteYousikiInfStatus.Failed => ResponseMessage.Failed,
        DeleteYousikiInfStatus.InvalidYousikiInf => ResponseMessage.InvalidYousikiInf,
        _ => string.Empty
    };
}
