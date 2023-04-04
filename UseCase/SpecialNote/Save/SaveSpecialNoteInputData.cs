using Domain.Models.SpecialNote.ImportantNote;
using Domain.Models.SpecialNote.SummaryInf;
using UseCase.Core.Sync.Core;

namespace UseCase.SpecialNote.Save
{
    public class SaveSpecialNoteInputData : IInputData<SaveSpecialNoteOutputData>
    {
        public SaveSpecialNoteInputData(int hpId, int ptId, SummaryInfModel summaryTab, ImportantNoteModel importantNoteTab, PatientInfoItem patientInfoTab, int userId)
        {
            HpId = hpId;
            PtId = ptId;
            SummaryTab = summaryTab;
            ImportantNoteTab = importantNoteTab;
            PatientInfoTab = patientInfoTab;
            UserId = userId;
        }

        public int HpId { get; private set; }
        public int PtId { get; private set; }
        public SummaryInfModel SummaryTab { get; private set; }

        public ImportantNoteModel ImportantNoteTab { get; private set; }

        public PatientInfoItem PatientInfoTab { get; private set; }

        public int UserId { get; private set; }
    }
}
