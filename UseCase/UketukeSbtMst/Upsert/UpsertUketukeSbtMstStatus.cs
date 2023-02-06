namespace UseCase.UketukeSbtMst.Upsert;

public enum UpsertUketukeSbtMstStatus
{
    InputNoData,
    Success = 1,
    Failed,
    InvalidKbnId,
    InvalidKbnName,
    InvalidSortNo,
    InvalidIsDeleted,
    UketukeListExistedInputData,
    UketukeListInvalidExistedKbnId,
}
