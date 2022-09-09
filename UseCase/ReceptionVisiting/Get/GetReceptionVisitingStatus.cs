

namespace UseCase.ReceptionVisiting.Get
{
    public enum GetReceptionVisitingStatus: byte
    {
        Success = 1,
        InvalidHpId = 2,
        InvalidPtId = 3,
        InvalidSinDate = 4
    }
}
