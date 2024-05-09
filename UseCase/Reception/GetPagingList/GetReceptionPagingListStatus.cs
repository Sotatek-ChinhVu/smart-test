namespace UseCase.Reception.GetPagingList;

public enum GetReceptionPagingListStatus : byte
{
    Success = 1,
    InvalidHpId,
    InvalidSinDate,
}
