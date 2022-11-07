namespace EmrCloudApi.Tenant.Requests.SpecialNote
{
    public partial class SpecialNoteSaveRequest
    {
        public int HpId { get; set; }
        public int PtId { get; set; }
        public SummaryInfRequest SummaryTab { get; set; } = new SummaryInfRequest();

        public ImportantNoteRequest ImportantNoteTab { get; set; } = new ImportantNoteRequest();

        public PatientInfoRequest PatientInfoTab { get; set; } = new PatientInfoRequest();
    }
}
