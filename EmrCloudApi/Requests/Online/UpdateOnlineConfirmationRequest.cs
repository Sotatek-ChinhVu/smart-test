namespace EmrCloudApi.Requests.Online
{
    public class UpdateOnlineConfirmationRequest
    {
        public string ReceptionNumber { get; set; } = string.Empty;

        public int YokakuDate { get; set; }

        public string QCBIDXmlMsgResponse { get; set; } = string.Empty;
    }
}
