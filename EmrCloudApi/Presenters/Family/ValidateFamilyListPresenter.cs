using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Family;
using UseCase.Family;
using UseCase.Family.ValidateFamilyList;

namespace EmrCloudApi.Presenters.Family;

public class ValidateFamilyListPresenter : IValidateFamilyListOutputPort
{
    public Response<ValidateFamilyListResponse> Result { get; private set; } = new();

    public void Complete(ValidateFamilyListOutputData output)
    {
        Result.Data = new ValidateFamilyListResponse(output.Status == ValidateFamilyListStatus.Successed);
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
        ValidateFamilyListStatus.InvalidBirthday => ResponseMessage.InvalidFamilyBirthday,
        ValidateFamilyListStatus.InvalidIsSeparated => ResponseMessage.InvalidFamilyIsSeparated,
        ValidateFamilyListStatus.InvalidBiko => ResponseMessage.InvalidFamilyBiko,
        ValidateFamilyListStatus.InvalidFamilyRekiId => ResponseMessage.InvalidFamilyRekiId,
        ValidateFamilyListStatus.InvalidByomeiCd => ResponseMessage.InvalidByomeiCd,
        ValidateFamilyListStatus.InvalidByomei => ResponseMessage.InvalidByomei,
        ValidateFamilyListStatus.InvalidCmt => ResponseMessage.InvalidFamilyCmt,
        ValidateFamilyListStatus.DuplicateFamily => ResponseMessage.DuplicateFamily,
        ValidateFamilyListStatus.InvalidNameMaxLength => ResponseMessage.InvalidNameMaxLength,
        ValidateFamilyListStatus.InvalidKanaNameMaxLength => ResponseMessage.InvalidKanaNameMaxLength,
        _ => string.Empty
    };
}