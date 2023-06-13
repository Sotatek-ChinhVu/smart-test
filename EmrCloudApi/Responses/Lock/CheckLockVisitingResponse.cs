using UseCase.Lock.Get;

namespace EmrCloudApi.Responses.Lock
{
    public class CheckLockVisitingResponse
    {
        public CheckLockVisitingResponse(CheckLockVisitingStatus status)
        {
            Status = status;
        }

        public CheckLockVisitingStatus Status { get; set; }
    }
}
