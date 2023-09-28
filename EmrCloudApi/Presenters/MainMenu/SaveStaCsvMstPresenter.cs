using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MainMenu;
using UseCase.MainMenu.SaveStaCsvMst;

namespace EmrCloudApi.Presenters.MainMenu;

public class SaveStaCsvMstPresenter : ISaveStaCsvMstOutputPort
{
    public Response<SaveStaCsvMstResponse> Result { get; private set; } = new();

    public void Complete(SaveStaCsvMstOutputData output)
    {
        Result.Data = new SaveStaCsvMstResponse(output.Status == SaveStaCsvMstStatus.Successed);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(SaveStaCsvMstStatus status) => status switch
    {
        SaveStaCsvMstStatus.Successed => ResponseMessage.Success,
        SaveStaCsvMstStatus.Failed => ResponseMessage.Failed,
        SaveStaCsvMstStatus.InvalidUserId => ResponseMessage.InvalidUserId,
        SaveStaCsvMstStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        SaveStaCsvMstStatus.InvalidConFName => ResponseMessage.InvalidConFName,
        SaveStaCsvMstStatus.InvalidColumnName => ResponseMessage.InvalidColumnName,
        _ => string.Empty
    };
}
