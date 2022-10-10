namespace UseCase.MstItem.FindTenMst
{
    public enum FindTenMstStatus : byte
    {
        Successed = 1,
        InValidHpId = 2,
        InvalidSindate = 3,
        InvalidItemCd = 4,
        Failed = 5
    }
}
