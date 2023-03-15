using UseCase.Receipt.GetInsuranceInf;

namespace EmrCloudApi.Responses.Receipt
{
    public class GetInsuranceInfResponse
    {
        public GetInsuranceInfResponse(List<InsuranceInfDto> insuranceInfDtos)
        {
            InsuranceInfDtos = insuranceInfDtos;
        }

        public List<InsuranceInfDto> InsuranceInfDtos { get; private set; }
    }
}
