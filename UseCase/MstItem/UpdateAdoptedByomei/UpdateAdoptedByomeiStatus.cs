namespace UseCase.MstItem.UpdateAdoptedByomei
{
    public enum UpdateAdoptedByomeiStatus : byte
    {
        Successed = 1,
        InvalidHospitalId = 2,
        InvalidByomeiCd = 3,
        Failed = 4
    }
}
