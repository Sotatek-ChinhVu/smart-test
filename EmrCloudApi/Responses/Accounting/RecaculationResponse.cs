using UseCase.Accounting.Recaculate;

namespace EmrCloudApi.Responses.Accounting
{
    public class RecaculationResponse
    {
        public RecaculationResponse(RecaculationStatus recaculationStatus)
        {
            RecaculationStatus = recaculationStatus;
        }

        public RecaculationStatus RecaculationStatus { get; private set; }
    }
}
