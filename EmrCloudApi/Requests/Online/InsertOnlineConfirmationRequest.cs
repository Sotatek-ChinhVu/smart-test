namespace EmrCloudApi.Requests.Online
{
    public class InsertOnlineConfirmationRequest
    {
        public int SinDate { get; private set; }

        public string QCBIXmlMsgRequest { get; private set; } = string.Empty;

        public string QCBIXmlMsgResponse { get; private set; } = string.Empty;
    }
}
