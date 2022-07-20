using Domain.Models.RaiinKubunMst;
using Entity.Tenant;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class RaiinKubunMstRepository : IRaiinKubunMstRepository
    {
        private readonly TenantDataContext _tenantDataContext;
        public RaiinKubunMstRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetDataContext();
        }

        public List<RaiinKubunMstModel> GetList(bool isDeleted)
        {
            List<RaiinKbnMst> raiinKubunMstList = _tenantDataContext.RaiinKbnMsts
                .Where(r => isDeleted || r.IsDeleted == 0)
                .OrderBy(r => r.SortNo)
                .ToList();

            List<int> groupIdList = raiinKubunMstList.Select(r => r.GrpCd).ToList();

            List<RaiinKbnDetail> raiinKubunDetailList = _tenantDataContext.RaiinKbnDetails
                .Where(r => groupIdList.Contains(r.GrpCd) && (isDeleted || r.IsDeleted == 0))
                .ToList();

            List<RaiinKubunMstModel> result = new();

            foreach (var raiinKubunMst in raiinKubunMstList)
            {
                int groupId = raiinKubunMst.GrpCd;

                List<RaiinKubunDetailModel> detailList = raiinKubunDetailList
                    .Where(r => r.GrpCd == groupId)
                    .Select(r => new RaiinKubunDetailModel(
                            r.GrpCd,
                            r.KbnCd,
                            r.SortNo,
                            r.KbnName,
                            r.ColorCd,
                            r.IsConfirmed == 1,
                            r.IsAuto == 1,
                            r.IsAutoDelete == 1,
                            r.IsDeleted == 1
                        ))
                    .ToList();

                result.Add(new RaiinKubunMstModel(
                        groupId,
                        raiinKubunMst.SortNo,
                        raiinKubunMst.GrpName,
                        raiinKubunMst.IsDeleted == 1,
                        detailList
                    ));
            }
            return result;
        }
    }
}
