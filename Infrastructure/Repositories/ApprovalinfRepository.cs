using Domain.Models.ApprovalInfo;
using Entity.Tenant;
using Helper.Constants;
using PostgreDataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UseCase.ApprovalInfo.GetApprovalInfList;

namespace Infrastructure.Repositories
{
    public class ApprovalinfRepository : IApprovalInfRepository
    {
        private readonly TenantDataContext _tenantTrackingDataContext;
        private readonly TenantNoTrackingDataContext _tenantNoTrackingDataContext;

        public List<ApprovalInfModel> GetList(int startDate, int endDate, int kaId, int tantoId)
        {
            var result = new List<ApprovalInfModel>();
                var approvalInfs = _tenantNoTrackingDataContext.ApprovalInfs.Where((x) =>
                        x.HpId == Session.HospitalID &&
                        x.SinDate >= startDate &&
                        x.SinDate <= endDate
                );
                var raiinInfs = _tenantNoTrackingDataContext.RaiinInfs.Where((x) =>
                        x.HpId == Session.HospitalID &&
                        x.IsDeleted == DeleteTypes.None &&
                        x.Status >= RaiinState.TempSave &&
                        x.SinDate >= startDate &&
                        x.SinDate <= endDate &&
                        (tantoId == 0 ? true : x.TantoId == tantoId) &&
                        (kaId == 0 ? true : x.KaId == kaId)
                );
                var ptInfs = _tenantNoTrackingDataContext.PtInfs.Where((x) =>
                        x.HpId == Session.HospitalID &&
                        x.IsDelete == 0
                );
                var kaMsts = _tenantNoTrackingDataContext.KaMsts.Where((x) =>
                        x.HpId == Session.HospitalID &&
                        x.IsDeleted == 0
                );
                var tantoInfs = _tenantNoTrackingDataContext.UserMsts.Where((x) =>
                        x.HpId == Session.HospitalID &&
                        x.IsDeleted == 0
                );

                var query = from raiinInf in raiinInfs
                            join approvalInf in approvalInfs on
                                new { raiinInf.RaiinNo, raiinInf.PtId, raiinInf.SinDate } equals
                                new { approvalInf.RaiinNo, approvalInf.PtId, approvalInf.SinDate } into approveInfList
                            join ptInf in ptInfs on
                                raiinInf.PtId equals ptInf.PtId
                            join kaMst in kaMsts on
                                raiinInf.KaId equals kaMst.KaId
                            join tantoInf in tantoInfs on
                                raiinInf.TantoId equals tantoInf.UserId
                            select new
                            {
                                ApprovalInf = approveInfList,
                                RaiinInf = raiinInf,
                                PtInf = ptInf,
                                TantoName = tantoInf.Sname,
                                KaName = kaMst.KaName
                            };

                result = query.AsEnumerable()
                    .Where(x => x.ApprovalInf.FirstOrDefault() == null
                             || x.ApprovalInf.OrderByDescending(m => m.SeqNo).FirstOrDefault().IsDeleted == 1)
                    .Select((x) => new ApprovalinfRepository()
                                .BuildApprovalInf(new ApprovalInf()
                                {
                                    SeqNo = x.ApprovalInf.FirstOrDefault() == null ? 1 : x.ApprovalInf.OrderByDescending(m => m.SeqNo).FirstOrDefault().SeqNo + 1,
                                    HpId = x.RaiinInf.HpId,
                                    PtId = x.RaiinInf.PtId,
                                    RaiinNo = x.RaiinInf.RaiinNo,
                                    SinDate = x.RaiinInf.SinDate,
                                    IsDeleted = 1
                                })
                                .BuildPtInf(x.PtInf)
                                .BuildRaiinInf(x.RaiinInf)
                                .BuildTantoName(x.TantoName)
                                .BuildKaName(x.KaName)
                                .Build()
                              )
                    .OrderBy((x) => x.SinDate)
                    .ToList();
            return result;
        }

        private ApprovalInf _approvalInf; 
        private PtInf _ptInf;
        private RaiinInf _raiinInf;
        private string _kaName;
        private string _tantoName;
        public IApprovalInfRepository BuildApprovalInf(ApprovalInf approvalInf)
        {
            _approvalInf = approvalInf;
            return this;
        }

        public IApprovalInfRepository BuildKaName(string kaName)
        {
            _kaName = kaName;
            return this;
        }

        public IApprovalInfRepository BuildPtInf(PtInf ptInf)
        {
            _ptInf = ptInf;
            return this;
        }

        public IApprovalInfRepository BuildRaiinInf(RaiinInf raiinInf)
        {
            _raiinInf = raiinInf;
            return this;
        }

        public IApprovalInfRepository BuildTantoName(string tantoName)
        {
            _tantoName = tantoName;
            return this;
        }

        public ApprovalInfModel Build()
        {
            return new ApprovalInfModel(_approvalInf, _ptInf, _raiinInf, _tantoName, _kaName);
        }
    }
}
