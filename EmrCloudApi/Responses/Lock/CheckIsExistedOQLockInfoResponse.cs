using Domain.Models.Lock;

namespace EmrCloudApi.Responses.Lock;

public class CheckIsExistedOQLockInfoResponse
{
    public CheckIsExistedOQLockInfoResponse(LockModel model)
    {
        LockInf = new LockInfModelDto(model);
    }

    public LockInfModelDto LockInf { get; private set; }
}
