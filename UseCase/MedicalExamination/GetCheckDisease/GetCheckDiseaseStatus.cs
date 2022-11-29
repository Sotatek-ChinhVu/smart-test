namespace UseCase.MedicalExamination.GetCheckDisease
{
    public enum GetCheckDiseaseStatus : byte
    {
        Successed = 1,
        InvalidHpId = 2,
        InvalidSinDate = 3,
        InvalidDrugOrByomei = 4,
        NoData = 5,
        Failed = 6
    }
}
