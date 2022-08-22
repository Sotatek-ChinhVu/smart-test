using UseCase.Core.Sync.Core;

namespace UseCase.SpecialNote.Get
{
    public class GetSpecialNoteOutputData : IOutputData
    {
        public GetSpecialNoteOutputData(SummaryTabItem? summaryTab, ImportantNoteTabItem? importantNoteTab, PatientInfoTabItem? patientInfoTab, GetSpecialNoteStatus status)
        {
            SummaryTab = summaryTab;
            ImportantNoteTab = importantNoteTab;
            PatientInfoTab = patientInfoTab;
            Status = status;
        }

        public SummaryTabItem? SummaryTab { get; private set; }
        public ImportantNoteTabItem? ImportantNoteTab { get; private set; }
        public PatientInfoTabItem? PatientInfoTab { get; private set; }
        public GetSpecialNoteStatus Status { get; private set; }
    }
}
