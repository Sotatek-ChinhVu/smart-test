using Domain.Models.ChartApproval;
using Domain.Models.User;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using static Helper.Constants.UserConst;


namespace Infrastructure.Repositories
{
    public class ApprovalinfRepository : RepositoryBase, IApprovalInfRepository
    {
        private readonly IUserRepository _userRepository;
        public ApprovalinfRepository(ITenantProvider tenantProvider, IUserRepository userRepository) : base(tenantProvider)
        {
            _userRepository = userRepository;
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
                var approvedInfo = NoTrackingDataContext.ApprovalInfs.Where(p => p.HpId == hpId && p.PtId == x.PtId && p.RaiinNo == x.RaiinNo && p.SinDate == x.SinDate).OrderByDescending(x => x.SeqNo).FirstOrDefault();
                if (approvedInfo == null)
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
                        SeqNo = 1,
                        UpdateId = userId,
                        UpdateDate = CIUtil.GetJapanDateTimeNow()
                    });
                }
                else
                {
                    var seqNo = approvedInfo.SeqNo + 1;

                    TrackingDataContext.ApprovalInfs.Add(new ApprovalInf()
                    {
                        HpId = hpId,
                        CreateDate = CIUtil.GetJapanDateTimeNow(),
                        CreateId = userId,
                        IsDeleted = 0,
                        PtId = x.PtId,
                        RaiinNo = x.RaiinNo,
                        SinDate = x.SinDate,
                        SeqNo = seqNo,
                        UpdateId = userId,
                        UpdateDate = CIUtil.GetJapanDateTimeNow()
                    });
                }
            });
            return TrackingDataContext.SaveChanges() > 0;
        }

        public bool NeedApprovalInf(int hpId, int startDate, int departmentId, int tantoId)
        {
            int endDate = 99999999;
            var approvalInfs = NoTrackingDataContext.ApprovalInfs.Where((x) =>
                                   x.HpId == hpId &&
                                   x.SinDate >= startDate &&
                                   x.SinDate <= endDate &&
                                   x.IsDeleted == 0);

            var raiinInfs = NoTrackingDataContext.RaiinInfs.Where((x) =>
                    x.HpId == hpId &&
                    x.IsDeleted == DeleteTypes.None &&
                    x.Status >= RaiinState.TempSave &&
                    x.SinDate >= startDate &&
                    x.SinDate <= endDate &&
                    (tantoId == 0 || x.TantoId == tantoId) &&
                    (departmentId == 0 || x.KaId == departmentId));

            var query = from raiinInf in raiinInfs
                        join approvalInf in approvalInfs on
                            new { raiinInf.RaiinNo, raiinInf.PtId, raiinInf.SinDate } equals
                            new { approvalInf.RaiinNo, approvalInf.PtId, approvalInf.SinDate } into approveInfList
                        from approvalInf in approveInfList.DefaultIfEmpty()
                        where approvalInf == null
                        select new
                        {
                            RaiinNo = raiinInf.RaiinNo,
                            ApprovalInf = approvalInf ?? new ApprovalInf()
                        };

            return query.AsEnumerable().Any(item => item.ApprovalInf.Id == 0);
        }

        public void UpdateApproveInf(int hpId, long ptId, int sinDate, long raiinNo, int userId)
        {
            bool authorized = _userRepository.GetPermissionByScreenCode(hpId, userId, FunctionCode.ApprovalInfo) == PermissionType.Unlimited;
            var approveInfList = TrackingDataContext.ApprovalInfs.Where(item => item.HpId == hpId &&
                                                                                item.PtId == ptId &&
                                                                                item.SinDate == sinDate &&
                                                                                item.RaiinNo == raiinNo)
                                                                  .ToList();

            if (authorized)
            {
                var approveInfActiveList = approveInfList.Where(item => item.IsDeleted == 0).ToList();
                if (approveInfActiveList.Any())
                {
                    // Approved
                    foreach (var approvedInf in approveInfActiveList)
                    {
                        approvedInf.UpdateId = userId;
                        approvedInf.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    }
                }
                else
                {
                    var approvingInfList = approveInfList.Where(x => x.IsDeleted == 1).ToList();
                    int seqNo = 0;
                    if (approvingInfList?.Any() != true)
                    {
                        seqNo = 1;
                    }
                    else
                    {
                        seqNo = approvingInfList.Max(x => x.SeqNo) + 1;
                    }
                    ApprovalInf newApprovalInf = new()
                    {
                        HpId = hpId,
                        PtId = ptId,
                        SinDate = sinDate,
                        RaiinNo = raiinNo,
                        IsDeleted = 0,
                        SeqNo = seqNo,
                        CreateId = userId,
                        UpdateId = userId,
                        CreateDate = CIUtil.GetJapanDateTimeNow(),
                        UpdateDate = CIUtil.GetJapanDateTimeNow(),
                    };
                    TrackingDataContext.ApprovalInfs.Add(newApprovalInf);
                }
            }
            else
            {
                var approvedInfs = approveInfList.Where(x => x.IsDeleted == 0).ToList();
                foreach (var approvedInf in approvedInfs)
                {
                    approvedInf.IsDeleted = 1;
                    approvedInf.UpdateId = userId;
                    approvedInf.UpdateDate = CIUtil.GetJapanDateTimeNow();
                }
            }
            TrackingDataContext.SaveChanges();
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}