using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Reception;
using UseCase.Reception.GetDefaultSelectedTime;

namespace EmrCloudApi.Presenters.Reception;

public class GetDefaultSelectedTimePresenter
{
    public Response<GetDefaultSelectedTimeResponse> Result { get; private set; } = new();

    public void Complete(GetDefaultSelectedTimeOutputData output)
    {
        Result.Data = new GetDefaultSelectedTimeResponse(output.Data);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetDefaultSelectedTimeStatus status) => status switch
    {
        GetDefaultSelectedTimeStatus.Successed => ResponseMessage.Success,
        GetDefaultSelectedTimeStatus.Failed => ResponseMessage.Failed,
        GetDefaultSelectedTimeStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        GetDefaultSelectedTimeStatus.InvalidBirthDay => ResponseMessage.InvalidBirthDay,
        GetDefaultSelectedTimeStatus.InvalidSinDate => ResponseMessage.InvalidSinDate,
        _ => string.Empty
    };
}
