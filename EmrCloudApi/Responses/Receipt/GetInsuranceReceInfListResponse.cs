using EmrCloudApi.Responses.Receipt.Dto;
using UseCase.Receipt.GetInsuranceReceInfList;

namespace EmrCloudApi.Responses.Receipt;

public class GetInsuranceReceInfListResponse
{
    public GetInsuranceReceInfListResponse(GetInsuranceReceInfListOutputData outputData)
    {
        OutputData = new InsuranceReceInfDto(outputData);
    }

    public InsuranceReceInfDto OutputData { get; private set; }
}
