namespace UseCase.SetKbn.GetList
{
    public enum GetSetKbnListStatus : byte
    {
        Successed = 1,
        NoData = 2,
        InvalidHpId = 3,
        InvalidSetKbnFrom = 4,
        InvalidSetKbnTo = 5,
        InvalidSetKbn = 6,
        InvalidSinDate = 7
    }
}