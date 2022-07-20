using Domain.Models.PatientCommentInfo;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    internal class PatientCommentInfoRepository : IPatientCommentInfoRepository
    {
        private readonly TenantDataContext _tenantDataContext;
        public PatientCommentInfoRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetDataContext();
        }
        public PatientCmtInfModel GetPatientCmtInf(long ptId)
        {
            var ptCmtInfEntity = _tenantDataContext.PtCmtInfs.SingleOrDefault(pt => pt.PtId == ptId);
            if (ptCmtInfEntity == null) return null;
            else return new PatientCmtInfModel(
                    ptCmtInfEntity.HpId,
                    ptCmtInfEntity.PtId,
                    ptCmtInfEntity.SeqNo,
                    ptCmtInfEntity.Text,
                    ptCmtInfEntity.IsDeleted,
                    ptCmtInfEntity.CreateDate,
                    ptCmtInfEntity.CreateId,
                    ptCmtInfEntity.CreateMachine,
                    ptCmtInfEntity.UpdateDate,
                    ptCmtInfEntity.UpdateId,
                    ptCmtInfEntity.UpdateMachine
                    );
        }
    }
}
