namespace EmrCloudApi.Requests.RaiinKubun
{
    public class GetPatientRaiinKubunListRequest
    {
        public long PtId { get; set; }

        public long RaiinNo { get; set; }

        public int SinDate { get; set; }
    }
}
