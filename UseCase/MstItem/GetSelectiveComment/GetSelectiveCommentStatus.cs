namespace UseCase.MstItem.GetSelectiveComment
{
    public enum GetSelectiveCommentStatus : byte
    {
        Successed = 1,
        InValidHpId = 2,
        InvalidSindate = 3,
        InvalidItemCds = 4,
        Failed = 5
    }
}
