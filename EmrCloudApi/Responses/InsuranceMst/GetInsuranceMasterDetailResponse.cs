using Domain.Models.InsuranceMst;

namespace EmrCloudApi.Responses.InsuranceMst
{
    public class GetInsuranceMasterDetailResponse
    {
        public IEnumerable<InsuranceMasterDto> InsuranceMstData { get; private set; }

        public GetInsuranceMasterDetailResponse(IEnumerable<InsuranceMasterDto> insuranceMstData)
        {
            InsuranceMstData = insuranceMstData;
        }
    }
}
