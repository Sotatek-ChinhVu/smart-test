namespace EmrCloudApi.Responses.ReceiptCheck
{
    public class RecalculationResponse
    {
        public RecalculationResponse(string errorText)
        {
            ErrorText = errorText;
        }

        public string ErrorText { get; private set; }
    }
}
