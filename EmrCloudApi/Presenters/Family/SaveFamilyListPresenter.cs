using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Family;
using UseCase.Family;
using UseCase.Family.SaveFamilyList;

namespace EmrCloudApi.Presenters.Family;

public class SaveFamilyListPresenter : ISaveFamilyListOutputPort
{
    public Response<SaveFamilyListResponse> Result { get; private set; } = new();

    public void Complete(SaveFamilyListOutputData output)
    {
        Result.Data = new SaveFamilyListResponse(output.Status == ValidateFamilyListStatus.Successed);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(ValidateFamilyListStatus status) => status switch
    {
        ValidateFamilyListStatus.Successed => ResponseMessage.Success,
        ValidateFamilyListStatus.Failed => ResponseMessage.Failed,
        ValidateFamilyListStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        ValidateFamilyListStatus.InvalidUserId => ResponseMessage.InvalidUserId,
        ValidateFamilyListStatus.InvalidPtIdOrFamilyPtId => ResponseMessage.InvalidPtIdOrFamilyPtId,
        ValidateFamilyListStatus.InvalidSortNo => ResponseMessage.InvalidSortNo,
        ValidateFamilyListStatus.InvalidFamilyId => ResponseMessage.InvalidFamilyId,
        ValidateFamilyListStatus.InvalidZokugaraCd => ResponseMessage.InvalidZokugaraCd,
        ValidateFamilyListStatus.InvalidName => ResponseMessage.InvalidFamilyName,
        ValidateFamilyListStatus.InvalidKanaName => ResponseMessage.InvalidFamilyKanaName,
        ValidateFamilyListStatus.InvalidSex => ResponseMessage.InvalidFamilySex,
        ValidateFamilyListStatus.InvalidBirthday => ResponseMessage.InvalidFamilyBirthday,
        ValidateFamilyListStatus.InvalidIsDead => ResponseMessage.InvalidFamilyIsDead,
        ValidateFamilyListStatus.InvalidIsSeparated => ResponseMessage.InvalidFamilyIsSeparated,
        ValidateFamilyListStatus.InvalidBiko => ResponseMessage.InvalidFamilyBiko,
        ValidateFamilyListStatus.InvalidFamilyRekiId => ResponseMessage.InvalidFamilyRekiId,
        ValidateFamilyListStatus.InvalidByomeiCd => ResponseMessage.InvalidByomeiCd,
        ValidateFamilyListStatus.InvalidByomei => ResponseMessage.InvalidByomei,
        ValidateFamilyListStatus.InvalidCmt => ResponseMessage.InvalidFamilyCmt,
        ValidateFamilyListStatus.DuplicateFamily => ResponseMessage.DuplicateFamily,
        _ => string.Empty
    };
}