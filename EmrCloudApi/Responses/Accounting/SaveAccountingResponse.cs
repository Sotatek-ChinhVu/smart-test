namespace EmrCloudApi.Responses.Accounting
{
    public class SaveAccountingResponse
    {
        public SaveAccountingResponse(bool success, List<long> raiinNoPrint)
        {
            Success = success;
            RaiinNoPrint = raiinNoPrint;
        }
        public bool Success { get; private set; }
        public List<long> RaiinNoPrint { get; private set; }
    }
}
