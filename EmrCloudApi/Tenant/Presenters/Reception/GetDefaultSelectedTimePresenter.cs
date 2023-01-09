using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Reception;
using UseCase.Reception.GetDefaultSelectedTime;

namespace EmrCloudApi.Tenant.Presenters.Reception;

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
        GetDefaultSelectedTimeStatus.InvalidUketukeTime => ResponseMessage.InvalidUketukeTime,
        _ => string.Empty
    };
}
