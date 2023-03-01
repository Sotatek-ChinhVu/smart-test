using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using UseCase.Receipt.GetDiseaseReceList;

namespace EmrCloudApi.Presenters.Receipt;

public class GetDiseaseReceListPresenter : IGetDiseaseReceListOutputPort
{
    public Response<GetDiseaseReceListResponse> Result { get; private set; } = new();

    public void Complete(GetDiseaseReceListOutputData outputData)
    {
        Result.Data = new GetDiseaseReceListResponse(outputData.DiseaseReceList);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(GetDiseaseReceListStatus status) => status switch
    {
        GetDiseaseReceListStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}
