using Domain.Models.Lock;

namespace EmrCloudApi.Responses.Lock;

public class CheckLockOpenAccountingResponse
{
    public CheckLockOpenAccountingResponse(bool status, List<LockModel> lockInfs)
    {
        Status = status;
        LockInfs = lockInfs;
    }

    public bool Status { get; private set; }

    public List<LockModel> LockInfs { get; private set; }
}
