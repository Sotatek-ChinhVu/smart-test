using UseCase.Core.Sync.Core;

namespace UseCase.Diseases.Validation
{
    public class ValidationPtDiseaseListInputData : IInputData<ValidationPtDiseaseListOutputData>
    {
        public ValidationPtDiseaseListInputData(List<ValidationPtDiseaseListInputItem> ptDiseaseModel, int hpId, int userId)
        {
            PtDiseaseModel = ptDiseaseModel;
            HpId = hpId;
            UserId = userId;
        }

        public List<ValidationPtDiseaseListInputItem> PtDiseaseModel { get; private set; }

        public int HpId { get; private set; }

        public int UserId { get; private set; }

        public List<ValidationPtDiseaseListInputItem> ToList()
        {
            return PtDiseaseModel;
        }

    }
}
