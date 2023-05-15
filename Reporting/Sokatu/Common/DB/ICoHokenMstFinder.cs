using Domain.Common;
using Reporting.Sokatu.Common.Models;

namespace Reporting.Sokatu.Common.DB
{
    public interface ICoHokenMstFinder : IRepositoryBase
    {
        List<CoKohiHoubetuMstModel> GetKohiHoubetuMst(int hpId, int seikyuYm);
    }
}
