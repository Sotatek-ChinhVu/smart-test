namespace EmrCloudApi.Requests.Online
{
    public class UpdateOnlineConfirmationRequest
    {
        public long RaiinNo { get; set; }

        public string ReceptionNumber { get; set; } = string.Empty;

        public string QCBIDXmlMsgResponse { get; set; } = string.Empty;
    }
}
