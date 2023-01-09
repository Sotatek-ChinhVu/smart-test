namespace UseCase.MedicalExamination.GetValidJihiYobo
{
    public enum GetValidJihiYoboStatus : byte
    {
        Successed = 1,
        InvalidHpId,
        InvalidSinDate,
        InvalidSyosaiKbn,
        Failed
    }
}
