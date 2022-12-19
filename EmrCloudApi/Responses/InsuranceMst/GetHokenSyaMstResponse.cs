using Domain.Models.InsuranceMst;

namespace EmrCloudApi.Responses.InsuranceMst
{
    public class GetHokenSyaMstResponse
    {
        public HokenSyaMstDto InsuranceSyaMst { get; private set; }

        public GetHokenSyaMstResponse(HokenSyaMstDto insuranceMst)
        {
            InsuranceSyaMst = insuranceMst;
        }
    }
}
