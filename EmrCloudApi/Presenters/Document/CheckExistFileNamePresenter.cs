using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Document;
using UseCase.Document.CheckExistFileName;

namespace EmrCloudApi.Presenters.Document;

public class CheckExistFileNamePresenter
{
    public Response<CheckExistFileNameResponse> Result { get; private set; } = new();

    public void Complete(CheckExistFileNameOutputData output)
    {
        Result.Data = new CheckExistFileNameResponse(output.Result);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(CheckExistFileNameStatus status) => status switch
    {
        CheckExistFileNameStatus.Successed => ResponseMessage.Success,
        CheckExistFileNameStatus.Failed => ResponseMessage.Failed,
        CheckExistFileNameStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        CheckExistFileNameStatus.InvalidPtId => ResponseMessage.InvalidPtId,
        CheckExistFileNameStatus.InvalidFileName => ResponseMessage.InvalidDocInfFileName,
        CheckExistFileNameStatus.InvalidCategoryCd => ResponseMessage.InvalidDocumentCategoryCd,
        _ => string.Empty
    };
}
