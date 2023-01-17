namespace UseCase.Family.SaveListFamily;

public enum SaveListFamilyStatus : byte
{
    Successed = 1,
    ValidateSuccess = 2,
    InvalidHpId = 3,
    InvalidUserId = 4,
    InvalidPtIdOrFamilyPtId = 5,
    InvalidSortNo = 6,
    InvalidFamilyId = 7,
    InvalidZokugaraCd = 8,
    InvalidName = 9,
    InvalidKanaName = 10,
    InvalidSex = 11,
    InvalidBirthday = 12,
    InvalidIsDead = 13,
    InvalidIsSeparated = 14,
    InvalidBiko = 15,
    InvalidFamilyRekiId = 16,
}
