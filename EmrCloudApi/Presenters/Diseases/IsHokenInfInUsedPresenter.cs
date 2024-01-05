using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Diseases;
using UseCase.Diseases.IsHokenInfInUsed;

namespace EmrCloudApi.Presenters.Diseases;

public class IsHokenInfInUsedPresenter : IIsHokenInfInUsedOutputPort
{
    public Response<IsHokenInfInUsedResponse> Result { get; private set; } = new();

    public void Complete(IsHokenInfInUsedOutputData output)
    {
        Result.Data = new IsHokenInfInUsedResponse(output.Result);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(IsHokenInfInUsedStatus status) => status switch
    {
        IsHokenInfInUsedStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}
