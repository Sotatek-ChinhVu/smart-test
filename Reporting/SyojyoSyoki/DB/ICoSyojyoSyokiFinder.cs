using Domain.Common;
using Reporting.SyojyoSyoki.Model;

namespace Reporting.SyojyoSyoki.DB
{
    public interface ICoSyojyoSyokiFinder : IRepositoryBase
    {
        List<CoSyojyoSyokiModel> FindSyoukiInf(int hpId, long ptId, int seikyuYm, int sinYm, int hokenId);
    }
}
