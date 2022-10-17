using Domain.Models.PatientInfor;

namespace EmrCloudApi.Tenant.Responses.PatientInfor.InsuranceMasterLinkage
{
    public class GetInsuranceMasterLinkageResponse
    {
        public GetInsuranceMasterLinkageResponse(List<DefHokenNoModel> defHokenNoModels)
        {
            DefHokenNoModels = defHokenNoModels;
        }

        public List<DefHokenNoModel> DefHokenNoModels { get; private set; }
    }
}
