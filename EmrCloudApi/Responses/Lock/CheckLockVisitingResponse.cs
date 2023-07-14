using Domain.Models.Lock;
using UseCase.Lock.Get;

namespace EmrCloudApi.Responses.Lock;

public class CheckLockVisitingResponse
{
    public CheckLockVisitingResponse(CheckLockVisitingStatus status, List<LockModel> lockInfs)
    {
        Status = status;
        LockInfs = lockInfs;
    }

    public CheckLockVisitingStatus Status { get; set; }

    public List<LockModel> LockInfs { get; private set; }
}
