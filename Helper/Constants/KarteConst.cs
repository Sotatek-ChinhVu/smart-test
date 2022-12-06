namespace Helper.Constants
{
    public static class KarteConst
    {
        public enum KarteValidationStatus
        {
            InvalidHpId = 1,
            InvalidRaiinNo,
            InvalidPtId,
            InvalidSinDate,
            InvalidIsDelted,
            RaiinNoNoExist,
            PtIdNoExist,
            Valid
        };

        public const int KarteKbn = 1;
    }
}
