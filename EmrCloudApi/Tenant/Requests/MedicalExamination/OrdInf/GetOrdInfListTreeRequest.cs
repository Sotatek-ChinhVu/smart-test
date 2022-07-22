namespace EmrCloudApi.Tenant.Requests.MedicalExamination.OrdInfs
{
    public class GetOrdInfListTreeRequest
    {
        public long PtId { get; set; }
        public int HpId { get; set; }
        public long RaiinNo { get; set; }
        public int SinDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
