namespace UseCase.Family;

public enum ValidateFamilyListStatus : byte
{
    ValidateSuccess = 0,
    Successed = 1,
    Failed,
    InvalidHpId,
    InvalidUserId,
    InvalidPtIdOrFamilyPtId,
    InvalidSortNo,
    InvalidFamilyId,
    InvalidZokugaraCd,
    InvalidBirthday,
    InvalidIsSeparated,
    InvalidBiko,
    InvalidFamilyRekiId,
    InvalidByomeiCd,
    InvalidByomei,
    InvalidCmt,
    DuplicateFamily,
    InvalidNameMaxLength,
    InvalidKanaNameMaxLength,
}
