using Domain.CalculationInf;
using Domain.Models.CalculationInf;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CalculationInfRepository: ICalculationInfRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;
        public CalculationInfRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetDataContext();
        }

        public IEnumerable<CalculationInfModel> GetListDataCalculationInf(int hpId, long ptId)
        {
            var dataCalculation = _tenantDataContext.PtSanteiConfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == 0)
                .Select( x => new CalculationInfModel(
                        x.HpId,
                        x.PtId,
                        x.KbnNo,
                        x.EdaNo,
                        x.KbnVal,
                        x.StartDate,
                        x.EndDate
                    ))
                .ToList();

            return dataCalculation;

        }
    }
}
