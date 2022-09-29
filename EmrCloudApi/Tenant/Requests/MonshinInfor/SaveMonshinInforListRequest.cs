using Domain.Models.MonshinInf;

namespace EmrCloudApi.Tenant.Requests.MonshinInfor
{
    public class SaveMonshinInforListRequest
    {
        public List<MonshinInforModel> Monshins { get; set; } = new();
    }
}
