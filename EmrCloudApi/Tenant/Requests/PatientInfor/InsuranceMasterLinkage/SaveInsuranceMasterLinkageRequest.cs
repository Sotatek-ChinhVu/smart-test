using Domain.Models.PatientInfor;

namespace EmrCloudApi.Tenant.Requests.PatientInfor.InsuranceMasterLinkage
{
    public class SaveInsuranceMasterLinkageRequest
    {
        public List<DefHokenNoModel> DefHokenNoModels { get; set; } = new();
    }
}
