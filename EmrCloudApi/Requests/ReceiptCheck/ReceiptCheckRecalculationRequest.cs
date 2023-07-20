namespace EmrCloudApi.Requests.ReceiptCheck
{
    public class ReceiptCheckRecalculationRequest
    {
        public List<long> PtIds { get; private set; } = new();

        public int SeikyuYm { get; private set; }
    }
}
