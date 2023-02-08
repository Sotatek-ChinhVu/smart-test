using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.GetHistoryIndex
{
    public class GetHistoryIndexOutputData : IOutputData
    {
        public GetHistoryIndexOutputData(GetHistoryIndexStatus status, long index)
        {
            Status = status;
            Index = index;
        }

        public GetHistoryIndexStatus Status { get; private set; }
        public long Index { get; private set; }
    }
}
