using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Reception;
using UseCase.Reception.Insert;

namespace EmrCloudApi.Tenant.Presenters.Reception;

public class InsertReceptionPresenter : IInsertReceptionOutputPort
{
    public Response<InsertReceptionResponse> Result { get; private set; } = new();

    public void Complete(InsertReceptionOutputData output)
    {
        Result.Data = new InsertReceptionResponse(output.Status == InsertReceptionStatus.Success);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(InsertReceptionStatus status) => status switch
    {
        InsertReceptionStatus.Success => ResponseMessage.Success,
        _ => string.Empty
    };
}
