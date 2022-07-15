using Domain.Models.InsuranceList;

namespace EmrCloudApi.Tenant.Responses.InsuranceList
{
    public class GetInsuranceListByIdResponse
    {
        public List<InsuranceListModel> Data { get; set; } = new List<InsuranceListModel>();
    }
}
