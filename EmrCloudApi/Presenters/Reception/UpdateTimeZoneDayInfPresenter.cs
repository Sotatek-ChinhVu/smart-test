using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Reception;
using UseCase.Reception.UpdateTimeZoneDayInf;

namespace EmrCloudApi.Presenters.Reception;

public class UpdateTimeZoneDayInfPresenter
{
    public Response<UpdateTimeZoneDayInfResponse> Result { get; private set; } = new();

    public void Complete(UpdateTimeZoneDayInfOutputData output)
    {
        Result.Data = new UpdateTimeZoneDayInfResponse(output.Status == UpdateTimeZoneDayInfStatus.Successed);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(UpdateTimeZoneDayInfStatus status) => status switch
    {
        UpdateTimeZoneDayInfStatus.Successed => ResponseMessage.Success,
        UpdateTimeZoneDayInfStatus.Failed => ResponseMessage.Failed,
        UpdateTimeZoneDayInfStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        UpdateTimeZoneDayInfStatus.InvalidUserId => ResponseMessage.UpsertInvalidUserId,
        UpdateTimeZoneDayInfStatus.InvalidSinDate => ResponseMessage.InvalidSinDate,
        UpdateTimeZoneDayInfStatus.InvalidCurrentTimeKbn => ResponseMessage.InvalidCurrentTimeKbn,
        UpdateTimeZoneDayInfStatus.InvalidBeforeTimeKbn => ResponseMessage.InvalidBeforeTimeKbn,
        UpdateTimeZoneDayInfStatus.InvalidUketukeTime => ResponseMessage.InvalidUketukeTime,
        UpdateTimeZoneDayInfStatus.CanNotUpdateTimeZoneInf => ResponseMessage.CanNotUpdateTimeZoneInf,
        _ => string.Empty
    };
}
