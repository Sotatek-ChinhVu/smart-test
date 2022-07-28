using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;

namespace EmrCloudApi.Tenant.Responses.InsuranceList
{
    public class GetInsuranceListResponse
    {
        public List<InsuranceModel> ListData { get; set; } = new List<InsuranceModel>();
    }
}