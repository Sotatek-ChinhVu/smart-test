using UseCase.Core.Sync.Core;

namespace UseCase.Diseases.Validation
{
    public class ValidationPtDiseaseListOutputData : IOutputData
    {
        public ValidationPtDiseaseListStatus Status { get; private set; }

        public ValidationPtDiseaseListOutputData(ValidationPtDiseaseListStatus status)
        {
            Status = status;
        }
    }
}
