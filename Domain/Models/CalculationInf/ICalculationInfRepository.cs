using Domain.Common;
using Domain.Models.CalculationInf;
using Domain.Models.Medical;
using Domain.Models.Receipt;
using Domain.Models.Receipt.Recalculation;

namespace Domain.CalculationInf
{
    public interface ICalculationInfRepository : IRepositoryBase
    {
        IEnumerable<CalculationInfModel> GetListDataCalculationInf(int hpId, long ptId);

        void CheckErrorInMonth(int hpId, int seikyuYm, List<long> ptIds);
    }
}
