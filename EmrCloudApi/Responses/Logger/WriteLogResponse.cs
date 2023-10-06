using UseCase.Logger;

namespace EmrCloudApi.Responses.Logger
{
    public class WriteLogResponse
    {
        public WriteLogResponse(WriteLogStatus status)
        {
            Status = status;
        }

        public WriteLogStatus Status { get; private set; }
    }
}
