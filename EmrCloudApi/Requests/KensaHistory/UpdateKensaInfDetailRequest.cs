using Domain.Models.KensaInfDetail;

namespace EmrCloudApi.Requests.KensaHistory
{
    public class UpdateKensaInfDetailRequest
    {
        public int HpId { get; set; }
        public int UserId { get; set; }
        public List<KensaInfDetailUpdateModel> kensaInfDetails { get; set; }
    }
}
