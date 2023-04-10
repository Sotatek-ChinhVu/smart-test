using Domain.Common;

namespace Domain.Models.HpInf
{
    public interface IHpInfRepository : IRepositoryBase
    {
        bool CheckHpId(int hpId);

        HpInfModel GetHpInf(int hpId);

        List<HpInfModel> GetListHpInf(int hpId);
    }
}
