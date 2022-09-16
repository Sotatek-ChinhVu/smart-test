namespace Helper.Constants
{
    public static class PtAlrgyDrugConst
    {
        public enum ValidationStatus
        {
            InvalidHpId = 0,
            InvalidPtId = 1,
            InvalidSeqNo = 2,
            InvalidSortNo = 3,
            InvalidStartDate = 4,
            InvalidEndDate = 5,
            InvalidIsDeleted = 6,
            Valid = 7
        };
    }
}
