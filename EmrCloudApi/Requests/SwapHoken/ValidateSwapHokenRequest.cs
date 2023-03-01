namespace EmrCloudApi.Requests.SwapHoken
{
    public class ValidateSwapHokenRequest
    {
        public long PtId { get; set; }

        public int HokenPid { get; set; }

        public int StartDate { get; set; }

        public int EndDate { get; set; }
    }
}
