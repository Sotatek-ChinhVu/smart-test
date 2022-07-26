using Domain.Models.FlowSheet;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class FlowSheetRepository : IFlowSheetRepository
    {
        private readonly TenantDataContext _tenantDataContext;
        public FlowSheetRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetDataContext();
        }
        public List<FlowSheetModel> GetListFlowSheet()
        {
            return new List<FlowSheetModel>();
        }
    }
}
