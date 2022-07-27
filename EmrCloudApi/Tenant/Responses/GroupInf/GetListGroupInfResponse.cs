using Domain.Models.GroupInf;
using UseCase.GroupInf.GetList;

namespace EmrCloudApi.Tenant.Responses.GroupInf
{
    public class GetListGroupInfResponse
    {
        public List<GroupInfModel> ListData { get; set; } = new List<GroupInfModel>();
    }
}
