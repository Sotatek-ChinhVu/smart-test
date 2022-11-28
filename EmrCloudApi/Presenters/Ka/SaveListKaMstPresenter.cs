using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Ka;
using UseCase.Ka.SaveList;

namespace EmrCloudApi.Presenters.Ka;

public class SaveListKaMstPresenter : ISaveKaMstOutputPort
{
    public Response<SaveListKaMstResponse> Result { get; private set; } = new();

    public void Complete(SaveKaMstOutputData output)
    {
        Result.Data = new SaveListKaMstResponse(output.Status == SaveKaMstStatus.Successed);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(SaveKaMstStatus status) => status switch
    {
        SaveKaMstStatus.Successed => ResponseMessage.Success,
        SaveKaMstStatus.Failed => ResponseMessage.Failed,
        SaveKaMstStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        SaveKaMstStatus.InvalidUserId => ResponseMessage.InvalidUserId,
        SaveKaMstStatus.InvalidKaId => ResponseMessage.InvalidKaId,
        SaveKaMstStatus.KaSnameMaxLength20 => ResponseMessage.KaSnameMaxLength20,
        SaveKaMstStatus.KaNameMaxLength40 => ResponseMessage.KaNameMaxLength40,
        SaveKaMstStatus.ReceKaCdNotFound => ResponseMessage.ReceKaCdNotFound,
        SaveKaMstStatus.CanNotDuplicateKaId => ResponseMessage.CanNotDuplicateKaId,
        _ => string.Empty
    };
}
