
using Domain.Models.ApprovalInfo;

namespace EmrCloudApi.Tenant.Responses.ApprovalInf
{
    public class GetApprovalInfListResponse
    {
        public List<ApprovalInfModel> ApprovalInfList { get; set; } = new List<ApprovalInfModel>();
    }
}
