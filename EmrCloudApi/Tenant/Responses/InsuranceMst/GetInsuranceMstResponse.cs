using Domain.Models.IsuranceMst;

namespace EmrCloudApi.Tenant.Responses.InsuranceMst
{
    public class GetInsuranceMstResponse
    {
        public InsuranceMstModel? InsuranceMst { get; private set; }

        public GetInsuranceMstResponse(InsuranceMstModel? insuranceMst)
        {
            InsuranceMst = insuranceMst;
        }

    }
}
