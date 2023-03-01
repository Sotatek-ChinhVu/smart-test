using Domain.CalculationInf;
using Domain.Models.CalculationInf;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class CalculationInfRepository : RepositoryBase, ICalculationInfRepository
    {
        public CalculationInfRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public IEnumerable<CalculationInfModel> GetListDataCalculationInf(int hpId, long ptId)
        {
            var dataCalculation = NoTrackingDataContext.PtSanteiConfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == 0)
                .Select(x => new CalculationInfModel(
                        x.HpId,
                        x.PtId,
                        x.KbnNo,
                        x.EdaNo,
                        x.KbnVal,
                        x.StartDate,
                        x.EndDate,
                        x.SeqNo
                    ))
                .ToList();

            return dataCalculation;

        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
