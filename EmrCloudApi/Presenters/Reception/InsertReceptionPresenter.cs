using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Reception;
using UseCase.Reception.Insert;

namespace EmrCloudApi.Presenters.Reception;

public class InsertReceptionPresenter : IInsertReceptionOutputPort
{
    public Response<InsertReceptionResponse> Result { get; private set; } = new();

    public void Complete(InsertReceptionOutputData output)
    {
        Result.Data = new InsertReceptionResponse(output.RaiinNo);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(InsertReceptionStatus status) => status switch
    {
        InsertReceptionStatus.Success => ResponseMessage.Success,
        InsertReceptionStatus.InvalidInsuranceList => ResponseMessage.InvalidInsuranceList,
        _ => string.Empty
    };
}
