namespace Helper.Constants
{
    public class NextOrderConst
    {
        public static string NextOrderHeader = "予約オーダー";
        public static string PeriodicOrderHeader = "定期オーダー";

        public static int DefaultRsvDate = 99999999;

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
