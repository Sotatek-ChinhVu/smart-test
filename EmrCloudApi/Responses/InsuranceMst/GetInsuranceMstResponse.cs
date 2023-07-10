using Domain.Models.InsuranceMst;

namespace EmrCloudApi.Responses.InsuranceMst
{
    public class GetInsuranceMstResponse
    {
        public InsuranceMstModel InsuranceMst { get; private set; }

        public int PrefNo { get; private set; }

        public GetInsuranceMstResponse(InsuranceMstModel insuranceMst, int prefNo)
        {
            InsuranceMst = insuranceMst;
            PrefNo = prefNo;
        }

    }
}
