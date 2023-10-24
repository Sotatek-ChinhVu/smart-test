using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.SetMst;
using UseCase.MainMenu.SaveOdrSet;

namespace EmrCloudApi.Presenters.SetMst;

public class SaveOdrSetPresenter : ISaveOdrSetOutputPort
{
    public Response<SaveOdrSetResponse> Result { get; private set; } = new();

    public void Complete(SaveOdrSetOutputData output)
    {
        Result.Data = new SaveOdrSetResponse(output.Status == SaveOdrSetStatus.Successed);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(SaveOdrSetStatus status) => status switch
    {
        SaveOdrSetStatus.Successed => ResponseMessage.Success,
        SaveOdrSetStatus.Failed => ResponseMessage.Failed,
        SaveOdrSetStatus.InvalidSetCd => ResponseMessage.InvalidSetCd,
        SaveOdrSetStatus.InvalidItemCd => ResponseMessage.InvalidItemCd,
        SaveOdrSetStatus.InvalidQuanlity => ResponseMessage.InvalidQuanlity,
        _ => string.Empty
    };
}
