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
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
        }

        public PatientInforModel? GetById(int hpId, long ptId, bool isDeleted = true)
        {
            var itemData = _tenantDataContext.PtInfs.Where(x => x.HpId == hpId && x.PtId == ptId && ( x.IsDelete != 1 || isDeleted)).FirstOrDefault();
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
            //Get ptMemo
            string memo = string.Empty;
            PtMemo? ptMemo = _tenantDataContext.PtMemos.Where(x => x.PtId == itemData.PtId).FirstOrDefault();
            if (ptMemo != null)
            {
                memo = ptMemo.Memo ?? string.Empty;
            }

            //Get lastVisitDate
            int lastVisitDate = 0;
            RaiinInf? raiinInf = _tenantDataContext.RaiinInfs.Where(r => r.PtId == itemData.PtId).OrderByDescending(r => r.SinDate).FirstOrDefault();
            if (raiinInf != null)
            {
                lastVisitDate = raiinInf.SinDate;
            }

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
                itemData.MainHokenPid,
                memo,
                lastVisitDate,
                itemData.Setanusi ?? String.Empty
                );
        }
    }
}