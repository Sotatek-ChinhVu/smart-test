namespace UseCase.MedicalExamination.InitKbnSetting
{
    public enum InitKbnSettingStatus : byte
    {
        Successed = 1,
        InvalidHpId,
        InvalidWindowType,
        InvalidFrameId,
        InvalidPtId,
        InvalidRaiinNo,
        InvalidSinDate,
        Failed
    }
}
