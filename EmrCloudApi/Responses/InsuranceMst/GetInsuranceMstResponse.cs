using Domain.Models.InsuranceMst;

namespace EmrCloudApi.Responses.InsuranceMst
{
    public class GetInsuranceMstResponse
    {
        public InsuranceMstModel InsuranceMst { get; private set; }

        public GetInsuranceMstResponse(InsuranceMstModel insuranceMst)
        {
            InsuranceMst = insuranceMst;
        }

    }
}
