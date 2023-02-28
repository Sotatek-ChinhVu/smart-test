using Domain.Common;

namespace Domain.Models.RsvInf
{
    public interface IRsvInfRepository : IRepositoryBase
    {
        public List<RsvInfModel> GetList(int hpId, long ptId, int sinDate);
    }
}
