using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using UseCase.Receipt.GetSinMeiInMonthList;

namespace EmrCloudApi.Presenters.Receipt;

public class GetSinMeiInMonthListPresenter : IGetSinMeiInMonthListOutputPort
{
    public Response<GetMedicalDetailsResponse> Result { get; private set; } = new();
    public void Complete(GetSinMeiInMonthListOutputData outputData)
    {
        Result.Data = new GetMedicalDetailsResponse(outputData.SinMeiModels, outputData.Holidays);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(GetSinMeiInMonthListStatus status) => status switch
    {
        GetSinMeiInMonthListStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}
