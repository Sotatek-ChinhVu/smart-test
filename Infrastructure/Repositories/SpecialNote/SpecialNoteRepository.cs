using Domain.Models.SpecialNote;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories.SpecialNote
{
    public class SpecialNoteRepository : ISpecialNoteRepository
    {
        private readonly TenantDataContext _tenantDataContextTracking;
        private readonly TenantDataContext _tenantDataContextNoTracking;

        public SpecialNoteRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContextTracking = tenantProvider.GetTrackingTenantDataContext();
            _tenantDataContextNoTracking = tenantProvider.GetNoTrackingDataContext();
        }
        public bool SaveSpecialNote()
        {

        }
    }
}
