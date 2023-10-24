namespace EmrCloudApi.Responses.MstItem
{
    public class IsKensaItemOrderingResponse
    {
        public IsKensaItemOrderingResponse(bool status)
        {
            Status = status;
        }

        public bool Status { get; private set; }
    }
}
