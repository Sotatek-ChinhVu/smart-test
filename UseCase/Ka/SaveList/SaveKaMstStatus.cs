namespace UseCase.Ka.SaveList;

public enum SaveKaMstStatus : byte
{
    Successed = 1,
    Failed = 2,
    InvalidHpId = 3,
    InvalidUserId = 4,
    InvalidKaId = 5,
    KaSnameMaxLength20 = 6,
    KaNameMaxLength40 = 7,
    ReceKaCdNotFound = 8,
    CanNotDuplicateKaId = 9
}
