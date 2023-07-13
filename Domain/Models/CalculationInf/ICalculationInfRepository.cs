using Domain.Common;
using Domain.Models.CalculationInf;
using Domain.Models.Receipt;
using Domain.Models.Receipt.Recalculation;

namespace Domain.CalculationInf
{
    public interface ICalculationInfRepository : IRepositoryBase
    {
        IEnumerable<CalculationInfModel> GetListDataCalculationInf(int hpId, long ptId);

        int GetCountReceInfs(int hpId, List<long> ptIds, int sinYm);

        List<ReceCheckOptModel> GetReceCheckOpts(int hpId);

        List<ReceInfModel> GetReceInfModels(int hpId, List<long> ptIds, int sinYM);
    }
}
