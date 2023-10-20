using Domain.Models.KensaInfDetail;

namespace EmrCloudApi.Requests.KensaHistory
{
    public class UpdateKensaInfDetailRequest
    {
        public int PtId { get; set; }

        public int IraiCd { get; set; }

        public int IraiDate { get; set; }

        public List<KensaInfDetailUpdateModel> kensaInfDetails { get; set; } = new();
    }
}
