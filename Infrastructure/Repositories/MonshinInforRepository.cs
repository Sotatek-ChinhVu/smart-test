using Domain.Models.MonshinInf;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using PostgreDataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Infrastructure.Repositories
{
    public class MonshinInforRepository : IMonshinInforRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;

        public MonshinInforRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
        }

        public List<MonshinInforModel> MonshinInforModels(int hpId, long ptId)
        {
            var monshinList = _tenantDataContext.MonshinInfo
                .Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == 0)
                .Select(x => new MonshinInforModel(
                x.HpId,
                x.PtId,
                x.RaiinNo,
                x.SinDate,
                x.Text))
                .ToList();
            return monshinList;
        }

        public void SaveList(List<MonshinInforModel> monshinInforModels, MonshinInfo monshinInfo)
        {
            var executionStrategy = _tenantDataContext.Database.CreateExecutionStrategy();
            executionStrategy.Execute(() =>
            {
                var monshinInfor = _tenantDataContext.Database.BeginTransaction();
                // Insert Monshin
                _tenantDataContext.MonshinInfo.Add(new MonshinInfo
                {
                    HpId = TempIdentity.HpId,
                    PtId = monshinInfo.PtId,
                    RaiinNo = monshinInfo.RaiinNo,
                    SinDate = monshinInfo.SinDate,
                    Text = monshinInfo.Text,
                    SeqNo = monshinInfo.SeqNo,
                    Rtext = monshinInfo.Rtext,
                    GetKbn = monshinInfo.GetKbn,
                    CreateId = TempIdentity.UserId,
                    CreateMachine = TempIdentity.ComputerName
                });
                _tenantDataContext.SaveChanges();

            });

            #region Helper methods
            #endregion
        }

    }
}
