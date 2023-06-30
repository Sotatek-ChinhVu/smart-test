using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.AccountDue;
using UseCase.AccountDue.IsNyukinExisted;

namespace EmrCloudApi.Presenters.AccountDue;

public class IsNyukinExistedPresenter : IIsNyukinExistedOutputPort
{
    public Response<IsNyukinExistedResponse> Result { get; private set; } = new();

    public void Complete(IsNyukinExistedOutputData output)
    {
        Result.Data = new IsNyukinExistedResponse(output.Success);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(IsNyukinExistedStatus status) => status switch
    {
        IsNyukinExistedStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}
