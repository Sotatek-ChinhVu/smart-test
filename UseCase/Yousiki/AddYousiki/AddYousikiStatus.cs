namespace UseCase.Yousiki.AddYousiki;

public enum AddYousikiStatus : byte
{
    ValidateSuccessed = 0,
    Successed = 1,
    Failed = 2,
    InvalidYousikiSinYm = 3,
    InvalidYousikiSelectDataType0 = 4,
    InvalidYousikiSelectDataType1 = 5,
    InvalidYousikiSelectDataType2 = 6,
    InvalidYousikiSelectDataType3 = 7,
    IsYousikiExist = 8,
    InvalidHealthInsuranceAccepted = 9,
}
