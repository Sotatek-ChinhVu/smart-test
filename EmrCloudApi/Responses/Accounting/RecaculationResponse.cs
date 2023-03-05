using UseCase.Accounting.Recaculate;

namespace EmrCloudApi.Responses.Accounting
{
    public class RecaculationResponse
    {
        public RecaculationResponse(RecaculationStatus status)
        {
            Status = status;
        }

        public RecaculationStatus Status { get; private set; }
    }
}
