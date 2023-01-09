using Domain.Common;
using Domain.Enum;

namespace Domain.Models.Diseases
{
    public interface IPtDiseaseRepository : IRepositoryBase
    {
        List<PtDiseaseModel> GetPatientDiseaseList(int hpId, long ptId, int sinDate, int hokenId, DiseaseViewType openFrom);

        List<PtDiseaseModel> GetListPatientDiseaseForReport(int hpId, long ptId, int hokenPid, int sinDate, bool tenkiByomei);

        List<long> Upsert(List<PtDiseaseModel> inputDatas, int hpId, int userId);
    }
}
