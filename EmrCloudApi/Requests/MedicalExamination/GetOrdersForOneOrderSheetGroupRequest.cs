namespace EmrCloudApi.Requests.MedicalExamination
{
    public class GetOrdersForOneOrderSheetGroupRequest
    {
        public long PtId { get; set; }

        public int HpId { get; set; }

        public int SinDate { get; set; }

        public int OdrKouiKbn { get; set; }

        public int GrpKouiKbn { get; set; }

        public int Offset { get; set; }

        public int Limit { get; set; }
    }
}
