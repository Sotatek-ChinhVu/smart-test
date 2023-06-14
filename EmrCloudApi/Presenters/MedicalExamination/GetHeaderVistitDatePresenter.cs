using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using UseCase.MedicalExamination.GetHeaderVistitDate;
using EmrCloudApi.Responses.MedicalExamination;

namespace EmrCloudApi.Presenters.MedicalExamination;

public class GetHeaderVistitDatePresenter : IGetHeaderVistitDateOutputPort
{
    public Response<GetHeaderVistitDateResponse> Result { get; private set; } = new();

    public void Complete(GetHeaderVistitDateOutputData output)
    {
        Result.Data = new GetHeaderVistitDateResponse(output.FirstDate, output.LastDate);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetHeaderVistitDateStatus status) => status switch
    {
        GetHeaderVistitDateStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}
