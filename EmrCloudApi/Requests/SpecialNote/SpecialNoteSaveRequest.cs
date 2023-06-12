namespace EmrCloudApi.Requests.SpecialNote
{
    public partial class SpecialNoteSaveRequest
    {
        public long PtId { get; set; }
        public int SinDate { get; set; }
        public SummaryInfRequest SummaryTab { get; set; } = new SummaryInfRequest();

        public ImportantNoteRequest ImportantNoteTab { get; set; } = new ImportantNoteRequest();

        public PatientInfoRequest PatientInfoTab { get; set; } = new PatientInfoRequest();
    }
}
