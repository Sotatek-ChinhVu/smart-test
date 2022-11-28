using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Reception;
using UseCase.Reception.Update;

namespace EmrCloudApi.Presenters.Reception;

public class UpdateReceptionPresenter : IUpdateReceptionOutputPort
{
    public Response<UpdateReceptionResponse> Result { get; private set; } = new();

    public void Complete(UpdateReceptionOutputData output)
    {
        Result.Data = new UpdateReceptionResponse(output.Status == UpdateReceptionStatus.Success);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(UpdateReceptionStatus status) => status switch
    {
        UpdateReceptionStatus.NotFound => ResponseMessage.NotFound,
        UpdateReceptionStatus.Success => ResponseMessage.Success,
        _ => string.Empty
    };
}
