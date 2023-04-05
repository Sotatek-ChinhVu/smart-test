using EmrCloudApi.Responses.InsuranceMst;

namespace EmrCloudApi.Requests.InsuranceMst
{
    public class SaveOrdInsuranceMstRequest
    {
        public List<InsuranceDetailDto> Insurances { get; set; } = new List<InsuranceDetailDto>();
    }
}
