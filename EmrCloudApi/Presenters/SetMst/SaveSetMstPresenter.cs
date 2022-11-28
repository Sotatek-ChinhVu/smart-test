using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.SetMst;
using UseCase.SetMst.SaveSetMst;

namespace EmrCloudApi.Presenters.SetMst;

public class SaveSetMstPresenter : ISaveSetMstOutputPort
{
    public Response<SaveSetMstResponse> Result { get; private set; } = new Response<SaveSetMstResponse>();

    public void Complete(SaveSetMstOutputData output)
    {
        Result.Data = new SaveSetMstResponse(output.setMstModel);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(SaveSetMstStatus status) => status switch
    {
        SaveSetMstStatus.Successed => ResponseMessage.Success,
        SaveSetMstStatus.Failed => ResponseMessage.Failed,
        SaveSetMstStatus.InvalidSindate => ResponseMessage.InvalidSinDate,
        SaveSetMstStatus.InvalidSetCd => ResponseMessage.InvalidSetCd,
        SaveSetMstStatus.InvalidSetKbn => ResponseMessage.InvalidSetKbn,
        SaveSetMstStatus.InvalidSetKbnEdaNo => ResponseMessage.InvalidSetKbnEdaNo,
        SaveSetMstStatus.InvalidGenarationId => ResponseMessage.InvalidGenarationId,
        SaveSetMstStatus.InvalidLevel1 => ResponseMessage.InvalidLevel1,
        SaveSetMstStatus.InvalidLevel2 => ResponseMessage.InvalidLevel2,
        SaveSetMstStatus.InvalidLevel3 => ResponseMessage.InvalidLevel3,
        SaveSetMstStatus.InvalidSetName => ResponseMessage.InvalidSetName,
        SaveSetMstStatus.InvalidWeightKbn => ResponseMessage.InvalidWeightKbn,
        SaveSetMstStatus.InvalidColor => ResponseMessage.InvalidColor,
        _ => string.Empty
    };
}
