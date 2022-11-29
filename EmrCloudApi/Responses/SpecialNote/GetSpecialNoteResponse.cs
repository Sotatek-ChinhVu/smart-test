using Domain.Models.SpecialNote.ImportantNote;
using Domain.Models.SpecialNote.PatientInfo;
using Domain.Models.SpecialNote.SummaryInf;
using UseCase.SpecialNote.Get;

namespace EmrCloudApi.Responses.SpecialNote
{
    public class GetSpecialNoteResponse
    {
        public GetSpecialNoteResponse(SummaryInfModel summaryTab, ImportantNoteModel importantNoteTab, PatientInfoModel patientInfoTab)
        {
            SummaryTab = summaryTab;
            ImportantNoteTab = importantNoteTab;
            PatientInfoTab = patientInfoTab;
        }

        public SummaryInfModel SummaryTab { get; private set; }

        public ImportantNoteModel ImportantNoteTab { get; private set; }

        public PatientInfoModel PatientInfoTab { get; private set; }
    }
}
