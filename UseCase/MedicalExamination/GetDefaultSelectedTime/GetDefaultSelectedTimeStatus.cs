namespace UseCase.MedicalExamination.GetDefaultSelectedTime
{
    public enum GetDefaultSelectedTimeStatus : byte
    {
        Successed = 1,
        InvalidDayOfWeek,
        InvalidSinDate,
        InvalidHpId,
        InvalidBirthDay,
        InvalidUketukeTime
    }
}
