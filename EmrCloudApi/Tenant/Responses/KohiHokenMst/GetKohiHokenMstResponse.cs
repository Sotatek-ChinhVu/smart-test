using Domain.Models.Insurance;
using Domain.Models.InsuranceMst;

namespace EmrCloudApi.Tenant.Responses.KohiHokenMst
{
    public class GetKohiHokenMstResponse
    {
        public GetKohiHokenMstResponse(HokenMstModel hokenMst)
        {
            HokenMst = hokenMst;
        }

        public HokenMstModel HokenMst { get; private set; }
    }
}
