namespace EmrCloudApi.Requests.PatientInfor
{
    public class GetByIdRequest
    {
        public long PtId { get; set; }

        public int SinDate { get; set; }

        public long RaiinNo { get; set; }

        public List<int> ListStatus { get; set; } = new();

        public bool IsShowKyuSeiName { get; set; } = false;
    }
}