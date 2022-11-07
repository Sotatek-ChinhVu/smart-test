using Domain.Models.Insurance;

namespace EmrCloudApi.Tenant.Requests.Insurance
{
    public class ValidateInsuranceRequest
    {
        public long PtId { get; set; }

        public int PtBirthday { get; set; }

        public int SinDate { get; set; }

        public List<ValidateInsuranceDto> ListInsurance { get; set; } = new List<ValidateInsuranceDto>();
    }

}
