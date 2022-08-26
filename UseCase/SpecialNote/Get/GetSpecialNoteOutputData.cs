using Domain.Models.SpecialNote.ImportantNote;
using Domain.Models.SpecialNote.PatientInfo;
using Domain.Models.SpecialNote.SummaryInf;
using UseCase.Core.Sync.Core;

namespace UseCase.SpecialNote.Get
{
    public class GetSpecialNoteOutputData : IOutputData
    {
        public GetSpecialNoteOutputData(SummaryInfModel summaryTab, ImportantNoteModel importantNoteTab, PatientInfoModel patientInfoTab, GetSpecialNoteStatus status)
        {
            SummaryTab = summaryTab;
            ImportantNoteTab = importantNoteTab;
            PatientInfoTab = patientInfoTab;
            Status = status;
        }

        public GetSpecialNoteOutputData(GetSpecialNoteStatus status)
        {
            SummaryTab = new SummaryInfModel();
            ImportantNoteTab = new ImportantNoteModel();
            PatientInfoTab = new PatientInfoModel();
            Status = status;
        }

        public SummaryInfModel SummaryTab { get; private set; }

        public ImportantNoteModel ImportantNoteTab { get; private set; }

        public PatientInfoModel PatientInfoTab { get; private set; }

        public GetSpecialNoteStatus Status { get; private set; }
    }
}
