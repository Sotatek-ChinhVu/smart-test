namespace EmrCloudApi.Requests.VisitingList
{
    public class GetReceptionLockRequest
    {
        public long SinDate { get; set; }
        public long PtId { get; set; }
        public long RaiinNo { get; set; }
        public string FunctionCd { get; set; } = string.Empty;
    }
}
