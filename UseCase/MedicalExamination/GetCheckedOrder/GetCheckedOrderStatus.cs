namespace UseCase.MedicalExamination.GetCheckedOrder
{
    public enum GetCheckedOrderStatus : byte
    {
        Successed = 1,
        InvalidHpId = 2,
        InvalidUserId = 3,
        InvalidSinDate = 4,
        InvalidHokenId = 5,
        InvalidPtId = 6,
        InvalidIBirthDay = 7,
        InvalidRaiinNo = 8,
        InvalidSyosaisinKbn = 9,
        InvalidOyaRaiinNo = 10,
        InvalidTantoId = 11,
        InvalidPrimaryDoctor = 12,
        Failed
    }
}