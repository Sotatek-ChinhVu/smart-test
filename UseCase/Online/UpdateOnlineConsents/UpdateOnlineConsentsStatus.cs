namespace UseCase.Online.UpdateOnlineConsents;

public enum UpdateOnlineConsentsStatus : byte
{
    ValidateSuccess = 0,
    Successed = 1,
    Failed = 2,
    InvalidPtId = 3,
    InvalidXmlFile = 4,
    InvalidOnlineConfirmationDate = 5,
}
