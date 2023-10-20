using Domain.Models.KensaInfDetail;

namespace EmrCloudApi.Requests.KensaHistory
{
    public class UpdateKensaInfDetailRequest
    {
        public List<KensaInfDetailUpdateModel> kensaInfDetails { get; set; }
    }
}
