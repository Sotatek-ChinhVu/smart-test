using Domain.CommonObject;
using Domain.Models.PatientInfor;
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

        public IEnumerable<PatientInfor> GetAll()
        {
            return _tenantDataContext.PtInfs.Select(x => new PatientInfor(
                x.HpId,
                x.PtId,
                x.ReferenceNo,
                x.SeqNo,
                x.PtNum,
                x.KanaName,
                x.Name,
                x.Sex,
                x.Birthday,
                x.LimitConsFlg,
                x.IsDead,
                x.DeathDate,
                x.HomePost,
                x.HomeAddress1,
                x.HomeAddress2,
                x.Tel1,
                x.Tel2,
                x.Mail,
                x.Setanusi,
                x.Zokugara,
                x.Job,
                x.RenrakuName,
                x.RenrakuPost,
                x.RenrakuAddress1,
                x.RenrakuAddress2,
                x.RenrakuTel,
                x.RenrakuMemo,
                x.OfficeName,
                x.OfficePost,
                x.OfficeAddress1,
                x.OfficeAddress2,
                x.OfficeTel,
                x.OfficeMemo,
                x.IsRyosyoDetail,
                x.PrimaryDoctor,
                x.IsTester,
                x.MainHokenPid
                ))
                .Take(100);
        }

        public PatientInfor? GetById(long ptId)
        {
            var itemData = _tenantDataContext.PtInfs.Where(x => x.PtId == ptId).FirstOrDefault();
            if (itemData == null)
                return null;
            else
                return new PatientInfor(
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
