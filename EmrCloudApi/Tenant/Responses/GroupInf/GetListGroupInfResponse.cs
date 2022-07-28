using Domain.Models.GroupInf;
using UseCase.GroupInf.GetList;

namespace EmrCloudApi.Tenant.Responses.GroupInf
{
    public class GetListGroupInfResponse
    {
        public GetListGroupInfResponse(List<GroupInfModel> listData)
        {
            ListData = listData;
        }

        public List<GroupInfModel> ListData { get; private set; }
    }
}
