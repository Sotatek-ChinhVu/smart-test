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
            throw new NotImplementedException();
        }

        public bool SaveList(List<MonshinInforModel> monshinInforModels, int hpId, long ptId, long raiinNo, int sinDate)
        {
            foreach (var model in monshinInforModels)
            {
                var monshinInfor = monshinInforModels.Find(x => x.HpId == model.HpId
                    && x.PtId == model.PtId
                    && x.RaiinNo == model.RaiinNo
                    && x.SinDate == model.SinDate
                    && x.IsDeleted == 0);
                if (monshinInfor is not null)
                {
                    monshinInfor.Text = model.Text;
                    monshinInfor.UpdateDate = DateTime.UtcNow;
                    monshinInfor.UpdateId = TempIdentity.UserId;
                    monshinInfor.UpdateMachine = TempIdentity.ComputerName;
                }
                else
                {
                    var newMonshinInfor = monshinInforModels.Select(m => ToEntity(m));
                    _tenantDataContext.MonshinInfo.AddRange(newMonshinInfor);
                }
            }
            _tenantDataContext.SaveChanges();
            return true;
        }

        private MonshinInfo ToEntity(MonshinInforModel model)
        {
            return new MonshinInfo
            {
                HpId = TempIdentity.HpId,
                PtId = model.PtId,
                RaiinNo = model.RaiinNo,
                SinDate = model.SinDate,
                Text = model.Text,
                SeqNo = model.SeqNo,
                GetKbn = model.GetKbn,
                IsDeleted = model.IsDeleted,
                CreateId = TempIdentity.UserId,
                CreateDate = model.CreateDate,
                CreateMachine = TempIdentity.ComputerName,
                UpdateDate = model.UpdateDate,
                UpdateId = TempIdentity.UserId,
                UpdateMachine = TempIdentity.ComputerName
            };
        }
    }
}
