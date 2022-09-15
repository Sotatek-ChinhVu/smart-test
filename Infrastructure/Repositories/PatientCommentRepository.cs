using Domain.Models.PatientComment;
using Entity.Tenant;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class PatientCommentRepository : IPatientCommentRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;

        public PatientCommentRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
        }

        public List<PatientCommentModel> PatientCommentModels(int hpId, long pdId)
        {
            var listData = _tenantDataContext.PtCmtInfs
                .Where(x => x.HpId == hpId & x.PtId == pdId & x.IsDeleted == 0)
                .Select(x => new PatientCommentModel(
                x.HpId,
                x.PtId,
                x.Text))
                .ToList();
            return listData;
        }
    }
}
