using UseCase.Core.Sync.Core;

namespace UseCase.Diseases.Upsert
{
    public class UpsertPtDiseaseListInputData : IInputData<UpsertPtDiseaseListOutputData>
    {
        public UpsertPtDiseaseListInputData(List<UpsertPtDiseaseListInputItem> ptDiseaseModel)
        {
            this.ptDiseaseModel = ptDiseaseModel;
        }

        public List<UpsertPtDiseaseListInputItem> ptDiseaseModel { get; private set; }

        public List<UpsertPtDiseaseListInputItem> ToList()
        {
            return this.ptDiseaseModel;
        }

    }
}
