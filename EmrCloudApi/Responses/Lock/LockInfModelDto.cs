using Domain.Models.Lock;

namespace EmrCloudApi.Responses.Lock;

public class LockInfModelDto
{
    public LockInfModelDto(LockModel model)
    {
        UserId = model.UserId;
        UserName = model.UserName;
        LockDateTime = model.LockDateTime;
        FunctionName = model.FunctionName;
        FunctionCode = model.FunctionCode;
    }

    public int UserId { get; private set; }

    public string UserName { get; private set; }

    public DateTime LockDateTime { get; private set; }

    public string FunctionName { get; private set; }

    public string FunctionCode { get; private set; }
}
