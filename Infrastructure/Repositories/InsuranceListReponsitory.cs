using Domain.CommonObject;
using Domain.Models.InsuranceList;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class InsuranceListReponsitory : IInsuranceListResponsitory
    {
        private readonly TenantDataContext _tenantDataContext;
        public InsuranceListReponsitory(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetDataContext();
        }

        public IEnumerable<InsuranceListModel> GetInsuranceListById(PtId ptId){
            return _tenantDataContext.PtHokenPatterns.Where(x => x.PtId == ptId.Value).Select(
                x => new InsuranceListModel(
                    HpId.From(x.HpId),
                    PtId.From(x.PtId),
                    HokenPid.From(x.HokenPid),
                    x.SeqNo,
                    x.HokenKbn,
                    x.HokenSbtCd,
                    x.HokenId,
                    x.Kohi1Id,
                    x.Kohi2Id,
                    x.Kohi3Id,
                    x.Kohi4Id,
                    x.HokenMemo,
                    x.EndDate
                    )).ToList();
        }
    }
}
