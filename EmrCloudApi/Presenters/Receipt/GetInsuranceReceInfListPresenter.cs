using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using UseCase.Receipt.GetInsuranceReceInfList;

namespace EmrCloudApi.Presenters.Receipt;

public class GetInsuranceReceInfListPresenter : IGetInsuranceReceInfListOutputPort
{
    public Response<GetInsuranceReceInfListResponse> Result { get; private set; } = new();

    public void Complete(GetInsuranceReceInfListOutputData outputData)
    {
        Result.Data = new GetInsuranceReceInfListResponse(outputData.InsuranceReceInf);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(GetInsuranceReceInfListStatus status) => status switch
    {
        GetInsuranceReceInfListStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}
