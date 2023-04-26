using Domain.Models.SpecialNote.ImportantNote;
using Domain.Models.SpecialNote.PatientInfo;
using Domain.Models.SpecialNote.SummaryInf;

namespace Domain.Models.SpecialNote
{
    public class SpecialNoteModel
    {
        public SpecialNoteModel(ImportantNoteModel importantNoteModel, PatientInfoModel patientInfoModel, SummaryInfModel summaryInfModel)
        {
            ImportantNoteModel = importantNoteModel;
            PatientInfoModel = patientInfoModel;
            SummaryInfModel = summaryInfModel;
        }

        public ImportantNoteModel ImportantNoteModel { get; private set; }

        public PatientInfoModel PatientInfoModel { get; private set; }

        public SummaryInfModel SummaryInfModel { get; private set; }
    }
}
