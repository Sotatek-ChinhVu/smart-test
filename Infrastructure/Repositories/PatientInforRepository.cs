using Domain.Models.PatientInfor;
using Domain.Models.PatientInfor.Domain.Models.PatientInfor;
using Entity.Tenant;
using Helper.Extendsions;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class PatientInforRepository : IPatientInforRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;
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
                return ConvertToModel(itemData);
        }

        public List<PatientInforModel> SearchSimple(string keyword, bool isContainMode)
        {
            long ptNum = keyword.AsLong();

            var ptInfList = _tenantDataContext.PtInfs
                .Where(p => p.IsDelete == 0 && (p.PtNum == ptNum || isContainMode && (p.KanaName.Contains(keyword) || p.Name.Contains(keyword))))
                .ToList();

            return ptInfList.Select(p => ConvertToModel(p)).ToList();
        }

        private PatientInforModel ConvertToModel(PtInf itemData)
        {
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
                itemData.HomePost ?? string.Empty,
                itemData.HomeAddress1 ?? string.Empty,
                itemData.HomeAddress2 ?? string.Empty,
                itemData.Tel1 ?? string.Empty,
                itemData.Tel2 ?? string.Empty,
                itemData.Mail ?? string.Empty,
                itemData.Setanusi ?? string.Empty,
                itemData.Zokugara ?? string.Empty,
                itemData.Job ?? string.Empty,
                itemData.RenrakuName ?? string.Empty,
                itemData.RenrakuPost ?? string.Empty,
                itemData.RenrakuAddress1 ?? string.Empty,
                itemData.RenrakuAddress2 ?? string.Empty,
                itemData.RenrakuTel ?? string.Empty,
                itemData.RenrakuMemo ?? string.Empty,
                itemData.OfficeName ?? string.Empty,
                itemData.OfficePost ?? string.Empty,
                itemData.OfficeAddress1 ?? string.Empty,
                itemData.OfficeAddress2 ?? string.Empty,
                itemData.OfficeTel ?? string.Empty,
                itemData.OfficeMemo ?? string.Empty,
                itemData.IsRyosyoDetail,
                itemData.PrimaryDoctor,
                itemData.IsTester,
                itemData.MainHokenPid
                );
        }
    }
}