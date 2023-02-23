using Domain.Common;
using Domain.Enum;

namespace Domain.Models.Diseases
{
    public interface IPtDiseaseRepository : IRepositoryBase
    {
        List<PtDiseaseModel> GetPatientDiseaseList(int hpId, long ptId, int sinDate, int hokenId, DiseaseViewType openFrom, bool isContiFiltered, bool isInMonthFiltered);

        List<PtDiseaseModel> GetListPatientDiseaseForReport(int hpId, long ptId, int hokenPid, int sinDate, bool tenkiByomei);

        List<PtDiseaseModel> GetByomeiInThisMonth(int hpId, int sinYm, long ptId, int hokenId);

        List<long> Upsert(List<PtDiseaseModel> inputDatas, int hpId, int userId);

        List<ByomeiSetMstModel> GetDataTreeSetByomei(int hpId, int sinDate);
    }
}
