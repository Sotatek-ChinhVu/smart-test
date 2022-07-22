namespace EmrCloudApi.Tenant.Requests.MedicalExamination.KarteInfs
{
    public class GetListKarteInfRequest
    {
        public long PtId { get; set; }
        public long RaiinNo { get; set; }
        public int SinDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
