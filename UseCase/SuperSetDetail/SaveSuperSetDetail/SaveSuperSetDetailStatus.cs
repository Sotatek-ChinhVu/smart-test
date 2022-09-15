namespace UseCase.SuperSetDetail.SaveSuperSetDetail;

public enum SaveSuperSetDetailStatus : byte
{
    Successed = 1,
    Failed = 2,
    SaveSetByomeiFailed = 3,
    SaveSetKarteInfFailed = 4,
    SaveSetOrderInfFailed = 5,
}
