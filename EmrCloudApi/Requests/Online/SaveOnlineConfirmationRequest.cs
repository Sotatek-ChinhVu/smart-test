namespace EmrCloudApi.Requests.Online
{
    public class SaveOnlineConfirmationRequest
    {
        public long RaiinNo { get; set; }

        public string QCBIDXmlMsgRequest { get; set; } = string.Empty;

        public string QCBIDXmlMsgResponse { get; set; } = string.Empty;
    }
}
