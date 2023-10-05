using Domain.Common;
using Domain.Models.MainMenu;

namespace Domain.Models.RsvInf
{
    public interface IRsvInfRepository : IRepositoryBase
    {
        public List<RsvInfModel> GetList(int hpId, long ptId, int sinDate);

        List<RsvInfToConfirmModel> GetListRsvInfToConfirmModel(int hpId, int sinDate);
    }
}
