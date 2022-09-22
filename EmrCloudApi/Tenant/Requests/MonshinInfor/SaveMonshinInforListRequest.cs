using Domain.Models.MonshinInf;

namespace EmrCloudApi.Tenant.Requests.MonshinInfor
{
    public class SaveMonshinInforListRequest
    {
        public List<MonshinInforModel> Monshins { get; set; } = new();
        public int HpId { get; set; }
        public long PtId { get; set; }
        public long RaiinNo { get; set; }
        public int SinDate { get; set; }
    }
}
