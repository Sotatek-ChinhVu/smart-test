using Domain.Models.MonshinInf;

namespace EmrCloudApi.Tenant.Responses.MonshinInfor
{
    public class GetMonshinInforListResponse
    {
        public GetMonshinInforListResponse(List<MonshinInforModel> monshinInforModels)
        {
            MonshinInforModels = monshinInforModels;
        }
        public List<MonshinInforModel> MonshinInforModels { get; private set; }
    }
}
