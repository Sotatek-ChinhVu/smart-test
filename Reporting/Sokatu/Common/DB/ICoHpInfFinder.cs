using Domain.Common;
using Reporting.Sokatu.Common.Models;

namespace Reporting.Sokatu.Common.DB
{
    public interface ICoHpInfFinder : IRepositoryBase
    {
        CoHpInfModel GetHpInf(int hpId, int seikyuYm);

        List<CoKaMstModel> GetKaMst(int hpId);
    }
}
