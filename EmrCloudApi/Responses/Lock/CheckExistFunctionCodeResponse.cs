using Domain.Models.Lock;

namespace EmrCloudApi.Responses.Lock;

public class CheckExistFunctionCodeResponse
{
    public CheckExistFunctionCodeResponse(LockModel model)
    {
        LockInf = new LockInfModelDto(model);
    }

    public LockInfModelDto LockInf { get; private set; }
}
