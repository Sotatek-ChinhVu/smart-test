using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Family;
using UseCase.Family.SaveListFamily;

namespace EmrCloudApi.Presenters.Family;

public class SaveListFamilyPresenter : ISaveListFamilyOutputPort
{
    public Response<SaveListFamilyResponse> Result { get; private set; } = new();

    public void Complete(SaveListFamilyOutputData output)
    {
        Result.Data = new SaveListFamilyResponse(output.Status == SaveListFamilyStatus.Successed);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(SaveListFamilyStatus status) => status switch
    {
        SaveListFamilyStatus.Successed => ResponseMessage.Success,
        SaveListFamilyStatus.Failed => ResponseMessage.Failed,
        SaveListFamilyStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        SaveListFamilyStatus.InvalidUserId => ResponseMessage.InvalidUserId,
        SaveListFamilyStatus.InvalidPtIdOrFamilyPtId => ResponseMessage.InvalidPtIdOrFamilyPtId,
        SaveListFamilyStatus.InvalidSortNo => ResponseMessage.InvalidSortNo,
        SaveListFamilyStatus.InvalidFamilyId => ResponseMessage.InvalidFamilyId,
        SaveListFamilyStatus.InvalidZokugaraCd => ResponseMessage.InvalidZokugaraCd,
        SaveListFamilyStatus.InvalidName => ResponseMessage.InvalidFamilyName,
        SaveListFamilyStatus.InvalidKanaName => ResponseMessage.InvalidFamilyKanaName,
        SaveListFamilyStatus.InvalidSex => ResponseMessage.InvalidFamilySex,
        SaveListFamilyStatus.InvalidBirthday => ResponseMessage.InvalidFamilyBirthday,
        SaveListFamilyStatus.InvalidIsDead => ResponseMessage.InvalidFamilyIsDead,
        SaveListFamilyStatus.InvalidIsSeparated => ResponseMessage.InvalidFamilyIsSeparated,
        SaveListFamilyStatus.InvalidBiko => ResponseMessage.InvalidFamilyBiko,
        SaveListFamilyStatus.InvalidFamilyRekiId => ResponseMessage.InvalidFamilyRekiId,
        SaveListFamilyStatus.InvalidByomeiCd => ResponseMessage.InvalidByomeiCd,
        SaveListFamilyStatus.InvalidByomei => ResponseMessage.InvalidByomei,
        SaveListFamilyStatus.InvalidCmt => ResponseMessage.InvalidFamilyCmt,
        SaveListFamilyStatus.DuplicateFamily => ResponseMessage.DuplicateFamily,
        _ => string.Empty
    };
}