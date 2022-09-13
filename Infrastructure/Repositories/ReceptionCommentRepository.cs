using Domain.Models.ReceptionComment;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ReceptionCommentRepository : IReceptionCommentRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;

        public ReceptionCommentRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
        }

        public List<ReceptionCommentModel> GetReceptionComments(long raiinNo)
        {
            var receptionComment = _tenantDataContext.RaiinCmtInfs
                .Where(x => x.RaiinNo == raiinNo)
                .Select(x => new ReceptionCommentModel(
                x.HpId,
                x.PtId,
                x.RaiinNo,
                x.Text ?? String.Empty))
                .ToList();
            return receptionComment;
        }
    }
}
