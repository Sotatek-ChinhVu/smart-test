using Domain.Common;

namespace Domain.Models.SpecialNote.SummaryInf
{
    public interface ISummaryInfRepository : IRepositoryBase
    {
        SummaryInfModel Get(int hpId, long ptId);
    }
}
