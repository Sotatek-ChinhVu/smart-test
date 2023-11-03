namespace Helper.Constants
{
    public class NextOrderConst
    {
        public const string NextOrderHeader = "予約オーダー";
        public const string PeriodicOrderHeader = "定期オーダー";

        public const int DefaultRsvDate = 99999999;

        public enum NextOrderStatus
        {
            InvalidRsvkrtNo = 1,
            InvalidRsvkrtKbn,
            InvalidRsvDate,
            InvalidRsvName,
            InvalidIsDeleted,
            InvalidSortNo,
            Valid
        }
    }
}
