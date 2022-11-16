using Domain.Enum;

namespace Domain.Models.Diseases
{
    public interface IPtDiseaseRepository
    {
        List<PtDiseaseModel> GetPatientDiseaseList(int hpId, long ptId, int sinDate, int hokenId, DiseaseViewType openFrom);
       
        List<PtDiseaseModel> GetListPatientDiseaseForReport(int hpId, long ptId, int hokenPid, int sinDate, bool tenkiByomei);
        
        void Upsert(List<PtDiseaseModel> inputDatas, int hpId, int userId);
    }
}
