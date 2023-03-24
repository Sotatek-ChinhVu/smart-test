using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Document;
using UseCase.Document.SaveDocInf;

namespace EmrCloudApi.Presenters.Document;

public class SaveDocInfPresenter : ISaveDocInfOutputPort
{
    public Response<SaveDocInfResponse> Result { get; private set; } = new();

    public void Complete(SaveDocInfOutputData output)
    {
        Result.Data = new SaveDocInfResponse(output.Status == SaveDocInfStatus.Successed);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(SaveDocInfStatus status) => status switch
    {
        SaveDocInfStatus.Successed => ResponseMessage.Success,
        SaveDocInfStatus.Failed => ResponseMessage.Failed,
        SaveDocInfStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        SaveDocInfStatus.InvalidUserId => ResponseMessage.InvalidUserId,
        SaveDocInfStatus.InvalidCategoryCd => ResponseMessage.InvalidDocumentCategoryCd,
        SaveDocInfStatus.InvalidFileInput => ResponseMessage.InvalidFileInput,
        SaveDocInfStatus.InvalidPtId => ResponseMessage.InvalidPtId,
        SaveDocInfStatus.InvalidSindate => ResponseMessage.InvalidSinDate,
        SaveDocInfStatus.InvalidDisplayFileName => ResponseMessage.InvalidDocInfFileName,
        SaveDocInfStatus.InvalidRaiinNo => ResponseMessage.InvalidRaiinNo,
        _ => string.Empty
    };
}
