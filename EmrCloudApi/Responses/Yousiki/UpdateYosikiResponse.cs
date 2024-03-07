using UseCase.Yousiki.UpdateYosiki;

namespace EmrCloudApi.Responses.Yousiki
{
    public class UpdateYosikiResponse
    {
        public UpdateYosikiResponse(UpdateYosikiStatus status, string message)
        {
            Status = status;
            Message = message;
        }

        public UpdateYosikiStatus Status { get; private set; }

        public string Message { get; private set; }
    }
}
