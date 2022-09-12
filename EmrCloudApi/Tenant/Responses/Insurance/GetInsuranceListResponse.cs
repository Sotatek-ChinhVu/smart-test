using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;

namespace EmrCloudApi.Tenant.Responses.InsuranceList
{
    public class GetInsuranceListResponse
    {
        public InsuranceDataModel Data { get; set; } = new InsuranceDataModel();
    }
}