using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Lock;
using UseCase.Lock.CheckExistFunctionCode;

namespace EmrCloudApi.Presenters.Lock;

public class CheckExistFunctionCodePresenter
{
    public Response<CheckExistFunctionCodeResponse> Result { get; private set; } = new();

    public void Complete(CheckExistFunctionCodeOutputData outputData)
    {
        Result.Data = new CheckExistFunctionCodeResponse(outputData.LockInf);
        Result.Message = outputData.Status == CheckExistFunctionCodeStatus.Successed ? ResponseMessage.Success : ResponseMessage.NoData;
        Result.Status = (int)outputData.Status;
    }
}
