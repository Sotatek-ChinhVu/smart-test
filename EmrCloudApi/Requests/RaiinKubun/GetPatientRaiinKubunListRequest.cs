namespace EmrCloudApi.Requests.RaiinKubun
{
    public class GetPatientRaiinKubunListRequest
    {
        public int HpId { get; set; }

        public long PtId { get; set; }

        public int RaiinNo { get; set; }

        public int SinDate { get; set; }
    }
}
