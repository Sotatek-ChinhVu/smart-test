using UseCase.Core.Sync.Core;
using UseCase.VisitingList.SaveSettings;

namespace UseCase.SpecialNote.Save
{
    public class SaveSpecialNoteOutputData : IOutputData
    {
        public SaveSpecialNoteOutputData(SaveSpecialNoteStatus status)
        {
            Status = status;
        }

        public SaveSpecialNoteStatus Status { get; private set; }
    }
}
