using Reporting.SyojyoSyoki.Model;

namespace Reporting.SyojyoSyoki.DB
{
    public interface ICoSyojyoSyokiFinder
    {
        List<CoSyojyoSyokiModel> FindSyoukiInf(int hpId, long ptId, int seikyuYm, int sinYm, int hokenId);
    }
}
