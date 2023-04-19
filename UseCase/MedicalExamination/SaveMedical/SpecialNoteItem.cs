using Domain.Models.SpecialNote.ImportantNote;
using System.Text.Json.Serialization;
using UseCase.SpecialNote.Save;

namespace UseCase.MedicalExamination.SaveMedical
{
    public class SpecialNoteItem
    {
        [JsonConstructor]
        public SpecialNoteItem(SummaryInfItem summaryTab, ImportantNoteModel importantNoteTab, PatientInfoItem patientInfoTab)
        {
            SummaryTab = summaryTab;
            ImportantNoteTab = importantNoteTab;
            PatientInfoTab = patientInfoTab;
        }

        public SpecialNoteItem()
        {
            SummaryTab = new();
            ImportantNoteTab = new();
            PatientInfoTab = new();
        }

        public SummaryInfItem SummaryTab { get; private set; }

        public ImportantNoteModel ImportantNoteTab { get; private set; }

        public PatientInfoItem PatientInfoTab { get; private set; }
    }
}
