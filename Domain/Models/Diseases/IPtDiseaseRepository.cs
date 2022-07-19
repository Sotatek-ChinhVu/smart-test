using Domain.Enum;

namespace Domain.Models.Diseases
{
    public interface IPtDiseaseRepository
    {
        IEnumerable<PtDisease> GetAllDiseaseInMonth(int hpId, long ptId, int sinDate, int hokenId, DiseaseViewType openFrom);
    }
}
