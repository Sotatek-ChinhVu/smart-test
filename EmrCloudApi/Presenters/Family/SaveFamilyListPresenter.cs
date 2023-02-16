using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Family;
using UseCase.Family.SaveListFamily;

namespace EmrCloudApi.Presenters.Family;

public class SaveFamilyListPresenter : ISaveFamilyListOutputPort
{
    public Response<SaveFamilyListResponse> Result { get; private set; } = new();

    public void Complete(SaveFamilyListOutputData output)
    {
        Result.Data = new SaveFamilyListResponse(output.Status == SaveFamilyListStatus.Successed);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(SaveFamilyListStatus status) => status switch
    {
        SaveFamilyListStatus.Successed => ResponseMessage.Success,
        SaveFamilyListStatus.Failed => ResponseMessage.Failed,
        SaveFamilyListStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        SaveFamilyListStatus.InvalidUserId => ResponseMessage.InvalidUserId,
        SaveFamilyListStatus.InvalidPtIdOrFamilyPtId => ResponseMessage.InvalidPtIdOrFamilyPtId,
        SaveFamilyListStatus.InvalidSortNo => ResponseMessage.InvalidSortNo,
        SaveFamilyListStatus.InvalidFamilyId => ResponseMessage.InvalidFamilyId,
        SaveFamilyListStatus.InvalidZokugaraCd => ResponseMessage.InvalidZokugaraCd,
        SaveFamilyListStatus.InvalidName => ResponseMessage.InvalidFamilyName,
        SaveFamilyListStatus.InvalidKanaName => ResponseMessage.InvalidFamilyKanaName,
        SaveFamilyListStatus.InvalidSex => ResponseMessage.InvalidFamilySex,
        SaveFamilyListStatus.InvalidBirthday => ResponseMessage.InvalidFamilyBirthday,
        SaveFamilyListStatus.InvalidIsDead => ResponseMessage.InvalidFamilyIsDead,
        SaveFamilyListStatus.InvalidIsSeparated => ResponseMessage.InvalidFamilyIsSeparated,
        SaveFamilyListStatus.InvalidBiko => ResponseMessage.InvalidFamilyBiko,
        SaveFamilyListStatus.InvalidFamilyRekiId => ResponseMessage.InvalidFamilyRekiId,
        SaveFamilyListStatus.InvalidByomeiCd => ResponseMessage.InvalidByomeiCd,
        SaveFamilyListStatus.InvalidByomei => ResponseMessage.InvalidByomei,
        SaveFamilyListStatus.InvalidCmt => ResponseMessage.InvalidFamilyCmt,
        SaveFamilyListStatus.DuplicateFamily => ResponseMessage.DuplicateFamily,
        _ => string.Empty
    };
}