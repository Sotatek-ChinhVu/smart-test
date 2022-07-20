using Domain.Models.PatientInfor;
using Domain.Models.PatientInfor.Domain.Models.PatientInfor;
using Domain.Models.User;
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
    public class PatientInforRepository : IPatientInforRepository
    {
        private readonly TenantDataContext _tenantDataContext;
        public PatientInforRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetDataContext();
        }

        public PatientInforModel? GetById(long ptId)
        {
            var itemData = _tenantDataContext.PtInfs.Where(x => x.PtId == ptId).FirstOrDefault();
            if (itemData == null)
                return null;
            else
                return new PatientInforModel(
                itemData.HpId,
                itemData.PtId,
                itemData.ReferenceNo,
                itemData.SeqNo,
                itemData.PtNum,
                itemData.KanaName,
                itemData.Name,
                itemData.Sex,
                itemData.Birthday,
                itemData.LimitConsFlg,
                itemData.IsDead,
                itemData.DeathDate,
                itemData.HomePost,
                itemData.HomeAddress1,
                itemData.HomeAddress2,
                itemData.Tel1,
                itemData.Tel2,
                itemData.Mail,
                itemData.Setanusi,
                itemData.Zokugara,
                itemData.Job,
                itemData.RenrakuName,
                itemData.RenrakuPost,
                itemData.RenrakuAddress1,
                itemData.RenrakuAddress2,
                itemData.RenrakuTel,
                itemData.RenrakuMemo,
                itemData.OfficeName,
                itemData.OfficePost,
                itemData.OfficeAddress1,
                itemData.OfficeAddress2,
                itemData.OfficeTel,
                itemData.OfficeMemo,
                itemData.IsRyosyoDetail,
                itemData.PrimaryDoctor,
                itemData.IsTester,
                itemData.MainHokenPid
                );
        }
    }
}