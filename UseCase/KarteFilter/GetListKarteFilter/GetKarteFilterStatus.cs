namespace UseCase.KarteFilter.GetListKarteFilter;

public enum GetKarteFilterStatus : byte
{
    Successed = 1,
    InvalidSinDate = 2,
    NoData = 3,
    Error = 4
}
