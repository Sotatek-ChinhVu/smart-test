using Domain.Models.Insurance;

namespace EmrCloudApi.Tenant.Requests.Insurance
{
    public class ValidateInsuranceRequest
    {
        public int HpId { get; set; }

        public int SinDate { get; set; }

        public int PtBirthday { get; set; }

        public List<ValidateInsuranceModel> ListDataModel { get; set; } = new List<ValidateInsuranceModel>();
    }
}
