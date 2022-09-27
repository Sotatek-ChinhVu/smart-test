namespace Helper.Constants
{
    public static class PtAlrgyDrugConst
    {
        public enum ValidationStatus
        {
            InvalidPtId = 0,
            InvalidSortNo = 1,
            InvalidStartDate = 2,
            InvalidEndDate = 3,
            InvalidItemCd = 4,
            InvalidDrugName = 5,
            InvalidCmt = 6,
            Valid = 7
        };
    }
}
