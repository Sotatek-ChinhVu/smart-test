namespace UseCase.MstItem.DeleteOrRecoverTenMst
{
    public enum DeleteOrRecoverTenMstStatus
    {
        Successful = 1,
        Failed,
        InvalidUserId,
        InvalidItemCd,
        RequiredConfirmDelete
    }
}
