namespace UseCase.MedicalExamination.KarteInfs.GetLists
{
    public enum GetListKarteInfStatus : byte
    {
        InvalidRaiinNo = 0,
        Successed = 1,
        InvalidPtId = 2,
        InvalidSinDate = 3,
        NoData = 4
    }
}
