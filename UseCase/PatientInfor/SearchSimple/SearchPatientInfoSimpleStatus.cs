namespace UseCase.PatientInfor.SearchSimple
{
    public enum SearchPatientInfoSimpleStatus : byte
    {
        NotFound = 0,
        Success = 1,
        InvalidKeyword = 2,
    }
}
