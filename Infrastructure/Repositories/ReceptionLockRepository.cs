using Domain.Models.LockInf;
using Domain.Models.ReceptionLock;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class ReceptionLockRepository : RepositoryBase, IReceptionLockRepository
    {
        public ReceptionLockRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public List<ReceptionLockModel> ReceptionLockModel(long sinDate, long ptId, long raiinNo, string functionCd)
        {
            var listData = NoTrackingDataContext.LockInfs
                .Where(x => x.SinDate == sinDate && x.PtId == ptId && x.RaiinNo == raiinNo && x.FunctionCd == functionCd)
                .Select(x => new ReceptionLockModel(
                x.HpId,
                x.PtId,
                x.FunctionCd ?? string.Empty,
                x.SinDate,
                x.RaiinNo,
                x.OyaRaiinNo,
                x.Machine ?? string.Empty,
                x.UserId,
                x.LockDate))
                .ToList();
            return listData;
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}