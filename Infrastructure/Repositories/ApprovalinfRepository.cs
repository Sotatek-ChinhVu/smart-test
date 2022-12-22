using Domain.Models.ApprovalInfo;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class ApprovalinfRepository : RepositoryBase, IApprovalInfRepository
    {
        public ApprovalinfRepository(ITenantProvider tenantProvider) : base(tenantProvider) 
        {
        }

        public bool CheckExistedId(List<int> ids)
        {
            var anyId = NoTrackingDataContext.ApprovalInfs.Any(u => ids.Contains(u.Id));
            return anyId;
        }
        public bool CheckExistedRaiinNo(List<long> raiinNo)
        {
            var anyRaiinNo = NoTrackingDataContext.ApprovalInfs.Any(u => raiinNo.Contains(u.RaiinNo));
            return anyRaiinNo;
        }
        public List<ApprovalInfModel> GetList(int hpId, int startDate, int endDate, int kaId, int tantoId)
        {
            var approvalInfs = NoTrackingDataContext.ApprovalInfs.Where((x) =>
                    x.HpId == hpId &&
                    x.SinDate >= startDate &&
                    x.SinDate <= endDate
            );
            var raiinInfs = NoTrackingDataContext.RaiinInfs.Where((x) =>
                    x.HpId == hpId &&
                    x.IsDeleted == DeleteTypes.None &&
                    x.Status >= RaiinState.TempSave &&
                    x.SinDate >= startDate &&
                    x.SinDate <= endDate &&
                    (tantoId == 0 || x.TantoId == tantoId) &&
                    (kaId == 0 || x.KaId == kaId)
            );
            var ptInfs = NoTrackingDataContext.PtInfs.Where((x) =>
                    x.HpId == hpId &&
                    x.IsDelete == 0
            );
            var kaMsts = NoTrackingDataContext.KaMsts.Where((x) =>
                    x.HpId == hpId &&
                    x.IsDeleted == 0
            );
            var userInfs = NoTrackingDataContext.UserMsts.Where((x) =>
                    x.HpId == hpId &&
                    x.IsDeleted == 0
            );

            var query = from raiinInf in raiinInfs.AsEnumerable()
                        join approvalInf in approvalInfs on
                            new { raiinInf.RaiinNo, raiinInf.PtId, raiinInf.SinDate } equals
                            new { approvalInf.RaiinNo, approvalInf.PtId, approvalInf.SinDate } into approveInfList
                        join ptInf in ptInfs on
                            raiinInf.PtId equals ptInf.PtId
                        join kaMst in kaMsts on
                            raiinInf.KaId equals kaMst.KaId
                        join userInf in userInfs on
                            raiinInf.TantoId equals userInf.UserId
                        select new
                        {
                            ApprovalInf = approveInfList,
                            RaiinInf = raiinInf,
                            PtInf = ptInf,
                            TantoName = userInf.Sname,
                            kaMst.KaName,
                            kaMst.KaId
                        };

            return query.Where(x => x.ApprovalInf.FirstOrDefault() == null
                         || x.ApprovalInf?.OrderByDescending(m => m.SeqNo).FirstOrDefault()?.IsDeleted == 1)
                .Select((x) => new ApprovalInfModel(
                            x.RaiinInf.HpId,
                            0,
                            x.RaiinInf.RaiinNo,
                            x.ApprovalInf.FirstOrDefault() == null ? 1 : x.ApprovalInf.OrderByDescending(m => m.SeqNo).FirstOrDefault()?.SeqNo ?? 0 + 1,
                            x.RaiinInf.PtId,
                            x.RaiinInf.SinDate,
                            1,
                            x.PtInf.PtNum,
                            x.KaName,
                            x.PtInf.Name ?? string.Empty,
                            x.KaId,
                            x.RaiinInf.UketukeNo
                      ))
            .OrderBy(x => x.SinDate)
            .ToList();
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
