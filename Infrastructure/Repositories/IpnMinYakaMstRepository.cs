using Domain.Models.IpnMinYakkaMst;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class IpnMinYakaMstRepository : IIpnMinYakaMstRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;
        public IpnMinYakaMstRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
        }

        public IpnMinYakkaMstModel? FindIpnMinYakkaMst(int hpId, string ipnNameCd, int sinDate)
        {
            var yakkaMst = _tenantDataContext.IpnMinYakkaMsts.Where(p =>
                  p.HpId == hpId &&
                  p.StartDate <= sinDate &&
                  p.EndDate >= sinDate &&
                  p.IpnNameCd == ipnNameCd)
              .FirstOrDefault();

            if (yakkaMst != null)
            {
                return new IpnMinYakkaMstModel(
                        yakkaMst.Id,
                        yakkaMst.HpId,
                        yakkaMst.IpnNameCd,
                        yakkaMst.StartDate,
                        yakkaMst.EndDate,
                        yakkaMst.Yakka,
                        yakkaMst.SeqNo,
                        yakkaMst.IsDeleted
                    );
            }
            return null;
        }
    }
}
