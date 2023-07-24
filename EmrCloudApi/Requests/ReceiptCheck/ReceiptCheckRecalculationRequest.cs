using Domain.Models.Receipt;

namespace EmrCloudApi.Requests.ReceiptCheck
{
    public class ReceiptCheckRecalculationRequest
    {
        public List<long> PtIds { get; set; } = new();

        public int SeikyuYm { get; set; }

        public ReceStatusModel ReceStatus { get; set; }
    }
}
