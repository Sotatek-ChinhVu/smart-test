using UseCase.SpecialNote.Get;

namespace EmrCloudApi.Tenant.Responses.SpecialNote
{
    public class GetSpecialNoteResponse
    {
        public GetSpecialNoteResponse(SummaryTabItem? summaryTab, ImportantNoteTabItem? importantNoteTab, PatientInfoTabItem? patientInfoTabItem)
        {
            SummaryTab = summaryTab;
            ImportantNoteTab = importantNoteTab;
            PatientInfoTabItem = patientInfoTabItem;
        }

        public SummaryTabItem? SummaryTab { get; private set; }
        public ImportantNoteTabItem? ImportantNoteTab { get; private set; }
        public PatientInfoTabItem? PatientInfoTabItem { get; private set; }
    }
}
