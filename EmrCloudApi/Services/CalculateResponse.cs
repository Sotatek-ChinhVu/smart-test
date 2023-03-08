using Helper.Enum;

namespace EmrCloudApi.Services
{
    public class CalculateResponse
    {
        public CalculateResponse(string responseMessage, ResponseStatus responseStatus)
        {
            ResponseMessage = responseMessage;
            ResponseStatus = responseStatus;
        }

        public string ResponseMessage { get; private set; }
        public ResponseStatus ResponseStatus { get; private set; }
    }
}
