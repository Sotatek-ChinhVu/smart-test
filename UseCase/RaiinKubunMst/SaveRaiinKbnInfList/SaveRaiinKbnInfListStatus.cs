namespace UseCase.RaiinKubunMst.SaveRaiinKbnInfList
{
    public enum SaveRaiinKbnInfListStatus : byte
    {
        InvalidHpId = 0,
        Successed = 1,
        InvalidSinDate = 2,
        InvalidRaiinNo = 3,
        InvalidUserId = 4,
        InvalidKbnInf = 5,
        InvalidPtId = 6,
        Failed = 7
    }
}
