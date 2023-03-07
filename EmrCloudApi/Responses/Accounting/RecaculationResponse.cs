using UseCase.Accounting.Recaculate;

namespace EmrCloudApi.Responses.Accounting
{
    public class RecaculationResponse
    {
        public RecaculationResponse(string message)
        {
            Message = message;
        }

        public string Message { get; private set; }
    }
}
