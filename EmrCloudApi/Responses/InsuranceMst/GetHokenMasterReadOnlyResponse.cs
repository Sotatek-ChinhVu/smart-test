using Domain.Models.InsuranceMst;

namespace EmrCloudApi.Responses.InsuranceMst
{
    public class GetHokenMasterReadOnlyResponse
    {
        public GetHokenMasterReadOnlyResponse(HokenMstModel hokenMaster)
        {
            HokenMaster = hokenMaster;
        }

        public HokenMstModel HokenMaster { get; private set; }
    }
}
