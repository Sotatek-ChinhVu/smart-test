using Domain.Models.ChartApproval;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using System;

namespace Infrastructure.Repositories
{
    public class ApprovalinfRepository : RepositoryBase, IApprovalInfRepository
    {
        public ApprovalinfRepository(ITenantProvider tenantProvider) : base(tenantProvider) 
        {

        }

        public List<ApprovalInfModel> GetList(int hpId, int startDate, int endDate, int kaId, int tantoId)
        {
            var approvalInfs = NoTrackingDataContext.ApprovalInfs.Where((x) => x.HpId == hpId &&
                                                                               x.SinDate >= startDate &&
                                                                               x.SinDate <= endDate);

            var raiinInfs = NoTrackingDataContext.RaiinInfs.Where((x) => x.HpId == hpId &&
                                                                         x.IsDeleted == DeleteTypes.None &&
                                                                         x.Status >= RaiinState.TempSave &&
                                                                         x.SinDate >= startDate &&
                                                                         x.SinDate <= endDate &&
                                                                         (tantoId == 0 || x.TantoId == tantoId) &&
                                                                         (kaId == 0 || x.KaId == kaId));

            var ptInfs = NoTrackingDataContext.PtInfs.Where((x) => x.HpId == hpId &&
                                                                   x.IsDelete == 0);

            var kaMsts = NoTrackingDataContext.KaMsts.Where((x) => x.HpId == hpId &&
                                                                   x.IsDeleted == 0);

            var userInfs = NoTrackingDataContext.UserMsts.Where((x) => x.HpId == hpId &&
                                                                       x.IsDeleted == 0);

            var queryList = (from raiinInf in raiinInfs
                             join ptInf in ptInfs on
                                raiinInf.PtId equals ptInf.PtId
                             join kaMst in kaMsts on
                                 raiinInf.KaId equals kaMst.KaId
                             join userInf in userInfs on
                                 raiinInf.TantoId equals userInf.UserId
                             select new
                             {
                                 ApprovalInf = approvalInfs.Where(x => x.RaiinNo == raiinInf.RaiinNo && x.PtId == raiinInf.PtId && x.SinDate == raiinInf.SinDate).OrderByDescending(m => m.SeqNo).FirstOrDefault(),
                                 RaiinInf = raiinInf,
                                 PtInf = ptInf,
                                 TantoName = userInf.Sname,
                                 KaName = kaMst.KaName,
                                 KaId = kaMst.KaId
                             }).ToList();


            return queryList
                   .Where(x => x.ApprovalInf == null || x.ApprovalInf.IsDeleted == DeleteTypes.Deleted)
                   .Select((x) => new ApprovalInfModel(
                                x.RaiinInf.HpId,
                                0,
                                x.RaiinInf.RaiinNo,
                                x.ApprovalInf == null ? 1 : x.ApprovalInf.SeqNo + 1,
                                x.RaiinInf.PtId,
                                x.RaiinInf.SinDate,
                                1,
                                x.PtInf.PtNum,
                                x.PtInf.KanaName ?? string.Empty,
                                x.PtInf.Name ?? string.Empty,
                                x.KaId,
                                x.RaiinInf.UketukeNo,
                                x.KaName,
                                x.TantoName))
                        .OrderBy(x => x.SinDate)
                        .ToList();
        }

        /// <summary>
        /// only have checked x => x.IsDeleted == 0
        /// index keys get
        /// </summary>
        /// <param name="approvalInfs"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool SaveApprovalInfs(List<ApprovalInfModel> approvalInfs, int hpId, int userId)
        {
            approvalInfs.ForEach(x =>
            {
                if(!NoTrackingDataContext.ApprovalInfs.Any(p => p.HpId == hpId && p.PtId == x.PtId && p.RaiinNo == x.RaiinNo && p.SinDate == x.SinDate))
                {
                    TrackingDataContext.ApprovalInfs.Add(new ApprovalInf()
                    {
                        HpId = hpId,
                        CreateDate = CIUtil.GetJapanDateTimeNow(),
                        CreateId = userId,
                        IsDeleted = x.IsDeleted,
                        PtId = x.PtId,
                        RaiinNo = x.RaiinNo,
                        SinDate = x.SinDate,
                        SeqNo = x.SeqNo,
                        UpdateId = userId,
                        UpdateDate = CIUtil.GetJapanDateTimeNow()
                    });
                }
            });
            return TrackingDataContext.SaveChanges() > 0;
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}