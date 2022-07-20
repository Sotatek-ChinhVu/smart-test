using Domain.Models.KensaInfDetail;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class KensaInfDetailRepository : IKensaInfDetailRepository
    {
        private readonly TenantDataContext _tenantDataContext;
        public KensaInfDetailRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetDataContext();
        }
        public List<KensaInfDetailModel> GetListByPtIdAndSinDate(long ptId, int sinDate)
        {
            return _tenantDataContext.KensaInfDetails
                .Where(p => p.PtId == ptId && p.IraiDate <= sinDate)
                .OrderByDescending(p => p.IraiDate)
                .Take(3)
                .Select(p => new KensaInfDetailModel(
                    p.HpId,
                    ptId,
                    p.IraiDate,
                    p.RaiinNo,
                    p.IraiCd,
                    p.SeqNo,
                    p.KensaItemCd,
                    p.ResultVal,
                    p.ResultType,
                    p.AbnormalKbn,
                    p.IsDeleted,
                    p.CmtCd1,
                    p.CmtCd2,
                    p.CreateDate,
                    p.CreateId,
                    p.CreateMachine,
                    p.UpdateDate,
                    p.UpdateId,
                    p.UpdateMachine)).ToList();
        }
    }
}
