using UseCase.Core.Sync.Core;

namespace UseCase.Diseases.Upsert
{
    public class UpsertPtDiseaseListInputData : IInputData<UpsertPtDiseaseListOutputData>
    {
        public UpsertPtDiseaseListInputData(List<UpsertPtDiseaseListInputItem> ptDiseaseModel, int hpId, int userId)
        {
            this.ptDiseaseModel = ptDiseaseModel;
            HpId = hpId;
            UserId = userId;
        }

        public List<UpsertPtDiseaseListInputItem> ptDiseaseModel { get; private set; }

        public int HpId { get; private set; }

        public int UserId { get; private set; }

        public List<UpsertPtDiseaseListInputItem> ToList()
        {
            return this.ptDiseaseModel;
        }

    }
}
