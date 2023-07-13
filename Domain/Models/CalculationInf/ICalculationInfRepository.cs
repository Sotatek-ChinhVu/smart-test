using Domain.Common;
using Domain.Models.CalculationInf;
using Domain.Models.Receipt.Recalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CalculationInf
{
    public interface ICalculationInfRepository : IRepositoryBase
    {
        IEnumerable<CalculationInfModel> GetListDataCalculationInf(int hpId, long ptId);

        int GetCountReceInfs(int hpId, List<long> ptIds, int sinYm);

        List<ReceCheckOptModel> GetReceCheckOpts(int hpId);
    }
}
