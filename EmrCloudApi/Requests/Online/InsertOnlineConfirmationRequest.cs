namespace EmrCloudApi.Requests.Online
{
    public class InsertOnlineConfirmationRequest
    {
        public int SinDate { get; set; }

        public string ArbitraryFileIdentifier { get; set; } = string.Empty;

        public string QCBIXmlMsgResponse { get; set; } = string.Empty;
    }
}
