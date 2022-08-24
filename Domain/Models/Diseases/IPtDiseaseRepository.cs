using Domain.Enum;

namespace Domain.Models.Diseases
{
    public interface IPtDiseaseRepository
    {
        List<PtDiseaseModel> GetPatientDiseaseList(int hpId, long ptId, int sinDate, int hokenId, DiseaseViewType openFrom);
        void Upsert(List<PtDiseaseModel> inputDatas);
    }
}
