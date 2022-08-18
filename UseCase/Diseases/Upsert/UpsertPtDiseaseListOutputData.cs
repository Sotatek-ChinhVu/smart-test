using UseCase.Core.Sync.Core;

namespace UseCase.Diseases.Upsert
{
    public class UpsertPtDiseaseListOutputData : IOutputData
    {
        public UpsertPtDiseaseListStatus Status { get; private set; }

        public UpsertPtDiseaseListOutputData(UpsertPtDiseaseListStatus status)
        {
            Status = status;
        }
    }
}
