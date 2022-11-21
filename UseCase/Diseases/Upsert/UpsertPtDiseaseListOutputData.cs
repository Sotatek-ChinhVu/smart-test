using UseCase.Core.Sync.Core;

namespace UseCase.Diseases.Upsert
{
    public class UpsertPtDiseaseListOutputData : IOutputData
    {

        public List<long> Ids { get; private set; }
        public UpsertPtDiseaseListStatus Status { get; private set; }

        public UpsertPtDiseaseListOutputData(UpsertPtDiseaseListStatus status, List<long> ids)
        {
            Status = status;
            Ids = ids;
        }
    }
}
