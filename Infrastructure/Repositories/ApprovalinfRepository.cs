using Domain.Models.ApprovalInfo;
using Domain.Models.User;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Infrastructure.Repositories
{
    public class ApprovalinfRepository : IApprovalInfRepository
    {
        private readonly TenantNoTrackingDataContext _tenantNoTrackingDataContext;
        private readonly TenantDataContext _tenantDataContext;

        public ApprovalinfRepository(ITenantProvider tenantProvider)
        {
            _tenantNoTrackingDataContext = tenantProvider.GetNoTrackingDataContext();
            _tenantDataContext = tenantProvider.GetTrackingTenantDataContext();
        }

        public List<ApprovalInfModel> GetList(int hpId, int startDate, int endDate, int kaId, int tantoId)
        {
            var result = new List<ApprovalInfModel>();
            var approvalInfs = _tenantNoTrackingDataContext.ApprovalInfs.Where((x) =>
                    x.HpId == hpId &&
                    x.SinDate >= startDate &&
                    x.SinDate <= endDate
            );
            var raiinInfs = _tenantNoTrackingDataContext.RaiinInfs.Where((x) =>
                    x.HpId == hpId &&
                    x.IsDeleted == DeleteTypes.None &&
                    x.Status >= RaiinState.TempSave &&
                    x.SinDate >= startDate &&
                    x.SinDate <= endDate &&
                    (tantoId == 0 || x.TantoId == tantoId) &&
                    (kaId == 0 || x.KaId == kaId)
            );
            var ptInfs = _tenantNoTrackingDataContext.PtInfs.Where((x) =>
                    x.HpId == hpId &&
                    x.IsDelete == 0
            );
            var kaMsts = _tenantNoTrackingDataContext.KaMsts.Where((x) =>
                    x.HpId == hpId &&
                    x.IsDeleted == 0
            );
            var userInfs = _tenantNoTrackingDataContext.UserMsts.Where((x) =>
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

            result = query.Where(x => x.ApprovalInf.FirstOrDefault() == null
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
                            x.PtInf.Name,
                            x.KaId,
                            x.RaiinInf.UketukeNo
                      ))
            .OrderBy(x => x.SinDate)
            .ToList();

            return result;
        }
        public void UpdateApprovalInfs(List<ApprovalInfModel> approvalInfs)
        {
            foreach (var inputData in approvalInfs)
            {
                var approvalInfo0 = _tenantDataContext.ApprovalInfs.FirstOrDefault(x => x.HpId == 1 && x.IsDeleted == 0);
                if(inputData.Id == approvalInfo0?.Id && inputData.IsDeleted == approvalInfo0.IsDeleted && inputData.RaiinNo == approvalInfo0.RaiinNo && inputData.PtId == approvalInfo0.PtId && inputData.SinDate == approvalInfo0.SinDate )
                {
                    approvalInfo0.CreateId = 1;
                    approvalInfo0.CreateDate = DateTime.Now;
                    approvalInfo0.CreateMachine = string.Empty;
                    approvalInfo0.UpdateId = 1;
                    approvalInfo0.UpdateDate = DateTime.Now;
                    approvalInfo0.UpdateMachine = string.Empty;
                }

                if(inputData.Id != approvalInfo0?.Id && inputData.RaiinNo != approvalInfo0?.RaiinNo)
                {
                    _tenantDataContext.ApprovalInfs.AddRange(ConvertApprovalInfList(inputData));
                }   

                if(inputData.Id == approvalInfo0?.Id && inputData.IsDeleted != approvalInfo0.IsDeleted)
                {
                    approvalInfo0.IsDeleted= inputData.IsDeleted;
                }    
            }
            _tenantDataContext.SaveChanges();
        }
        private static ApprovalInf ConvertApprovalInfList(ApprovalInfModel u)
        {
            return new ApprovalInf
            {
                Id = u.Id,
                HpId = u.HpId,
                IsDeleted = u.IsDeleted,
                RaiinNo = u.RaiinNo,
                SeqNo = u.SeqNo,
                PtId = u.PtId,
                SinDate = u.SinDate,
                CreateId = u.CreateId,
                CreateDate = u.CreateDate,
                CreateMachine = u.CreateMachine ?? string.Empty,
                UpdateId = u.UpdateId,
                UpdateDate = u.UpdateDate,
                UpdateMachine = u.UpdateMachine ?? string.Empty,
            };
        }
    }
}
