using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using UseCase.Receipt.Recalculation;

namespace EmrCloudApi.Presenters.Receipt;

public class RecalculationPresenter : IRecalculationOutputPort
{
    public Response<RecalculationResponse> Result { get; private set; } = new();

    public void Complete(RecalculationOutputData output)
    {
        Result.Data = new RecalculationResponse(output.ErrorMessage);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(RecalculationStatus status) => status switch
    {
        RecalculationStatus.Successed => ResponseMessage.Success,
        RecalculationStatus.Failed => ResponseMessage.Failed,
        RecalculationStatus.InvalidPtId => ResponseMessage.InvalidPtId,
        _ => string.Empty
    };
}
