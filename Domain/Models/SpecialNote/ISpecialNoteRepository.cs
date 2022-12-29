using Domain.Common;
using Domain.Models.SpecialNote.ImportantNote;
using Domain.Models.SpecialNote.PatientInfo;
using Domain.Models.SpecialNote.SummaryInf;

namespace Domain.Models.SpecialNote
{
    public interface ISpecialNoteRepository : IRepositoryBase
    {
        bool SaveSpecialNote(int hpId, long ptId, SummaryInfModel summaryInfModel, ImportantNoteModel importantNoteModel, PatientInfoModel patientInfoModel, int userId);
    }
}
