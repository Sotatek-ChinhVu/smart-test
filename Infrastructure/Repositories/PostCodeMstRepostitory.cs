using Domain.Models.PostCodeMst;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class PostCodeMstRepostitory : IPostCodeMstRepository
    {
        private readonly TenantDataContext _tenantDataContextTracking;
        private readonly TenantDataContext _tenantDataContextNoTracking;

        public PostCodeMstRepostitory(ITenantProvider tenantProvider)
        {
            _tenantDataContextTracking = tenantProvider.GetTrackingTenantDataContext();
            _tenantDataContextNoTracking = tenantProvider.GetNoTrackingDataContext();
        }

        //public List<PostCodeMstModel> PostCodeMstModels(string postCode1, string postCode2, string address)
        //{
        //    var entities = _tenantDataContextNoTracking.PostCodeMsts;
        //    if (!string.IsNullOrEmpty(postCode1))
        //    {

        //    }
        //}
    }
}
