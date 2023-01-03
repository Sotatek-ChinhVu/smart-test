namespace UseCase.RaiinKbn.GetPatientRaiinKubunList
{
    public enum GetPatientRaiinKubunListStatus : byte
    {

        InvalidSinDate = 5,
        InvalidRaiinNo = 4,
        InvalidPtId = 3,
        InvalidHpId = 2,
        Successed = 1,
    }
}
