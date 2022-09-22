using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Reception;
using UseCase.Reception.Update;

namespace EmrCloudApi.Tenant.Presenters.Reception;

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
