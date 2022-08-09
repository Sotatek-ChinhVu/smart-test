using Domain.Constant;
using Domain.Models.Insurance;
using Domain.Models.User;
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

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;
        public UserRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
        }

        public bool CheckExistedId(List<long> idList)
        {
            return _tenantDataContext.UserMsts.Any(u => idList.Contains(u.Id));
        }

        public void Create(UserMstModel user)
        {
            throw new NotImplementedException();
        }

        public void Delete(UserId userId)
        {
            throw new NotImplementedException();
        }

        public List<UserMstModel> GetAll(int sinDate, bool isDoctorOnly)
        {
            var query = _tenantDataContext.UserMsts.Where(u =>
                u.StartDate <= sinDate
                && u.EndDate >= sinDate
                && u.IsDeleted == DeleteTypes.None);
            if (isDoctorOnly)
            {
                query = query.Where(u => u.JobCd == JobCodes.Doctor);
            }

            //rain infor
            var hpId = 1;
            long ptId = 99999546;
            var listDataRaiinInf = _tenantDataContext.RaiinInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.SinDate == sinDate && x.IsDeleted == DeleteTypes.None).ToList();

            var _doctors = _tenantDataContext.UserMsts.Where(p => p.StartDate <= sinDate && p.EndDate >= sinDate && p.JobCd == 1).OrderBy(p => p.SortNo).ToList();
            var _departments = _tenantDataContext.KaMsts.Where(p => p.HpId == hpId && p.IsDeleted == DeleteTypes.None).ToList();
            var _comments = _tenantDataContext.RaiinCmtInfs.Where(p => p.HpId == hpId && p.PtId == ptId && p.SinDate == sinDate && p.IsDelete == DeleteTypes.None).ToList();
            var _timePeriodModels = _tenantDataContext.UketukeSbtMsts.Where(p => p.HpId == hpId && p.IsDeleted == DeleteTypes.None).OrderBy(p => p.SortNo).ToList();


            var raiinInfRespo =
                   dbService.RaiinInfRepository.FindListQueryable(item =>
                       (item.PtId == ptId) &&
                       item.HpId == hpId &&
                       item.IsDeleted == DeleteTypes.None &&
                       (fromDate == toDate
                           ? item.SinDate == fromDate
                           : (item.SinDate >= fromDate && item.SinDate <= toDate)));



            _departments = _doraiFinder.GetKaNameByHpId();
            _comments = _doraiFinder.GetRaiinCmtByPtId(_ptId, _sinday, _raiinNo);
            _hokenPattern = _doraiFinder.GetHokenPattern(_sinday, _sinday, _ptId);
            _timePeriodModels = _doraiFinder.GetTimePeriodModels();

            if (listDataRaiinInf != null && listDataRaiinInf.Count > 0)
            {
                foreach (var item in listDataRaiinInf)
                {

                }
            }



            return query.OrderBy(u => u.SortNo).AsEnumerable().Select(u => ToModel(u)).ToList();
        }

        private string NenkinBango(string? rousaiKofuNo)
        {
            string nenkinBango = "";
            if (rousaiKofuNo != null && rousaiKofuNo.Length == 9)
            {
                nenkinBango = rousaiKofuNo.Substring(0, 2);
            }
            return nenkinBango;
        }

        private int GetConfirmDate(int hokenId, int typeHokenGroup)
        {
            var validHokenCheck = _tenantDataContext.PtHokenChecks.Where(x => x.IsDeleted == 0 && x.HokenId == hokenId && x.HokenGrp == typeHokenGroup)
                .OrderByDescending(x => x.CheckDate)
                .ToList();
            if (!validHokenCheck.Any())
            {
                return 0;
            }
            return DateTimeToInt(validHokenCheck[0].CheckDate);
        }

        private static int DateTimeToInt(DateTime dateTime, string format = "yyyyMMdd")
        {
            int result = 0;
            result = Int32.Parse(dateTime.ToString(format));
            return result;
        }

        public UserMstModel? GetByUserId(int userId)
        {
            var entity = _tenantDataContext.UserMsts
                .Where(u => u.UserId == userId && u.IsDeleted == DeleteTypes.None).FirstOrDefault();
            return entity is null ? null : ToModel(entity);
        }

        public int MaxUserId()
        {
            return _tenantDataContext.UserMsts.Max(u => u.UserId);
        }

        public UserMstModel Read(UserId userId)
        {
            throw new NotImplementedException();
        }

        public void Update(UserMstModel user)
        {
            throw new NotImplementedException();
        }

        public void Upsert(List<UserMstModel> updatedUserList, List<UserMstModel> inserteddUserList)
        {

        }

        private UserMstModel ToModel(UserMst u)
        {
            return new UserMstModel(
                u.Id,
                u.UserId,
                u.JobCd,
                u.ManagerKbn,
                u.KaId,
                u.KanaName ?? string.Empty,
                u.Name ?? string.Empty,
                u.Sname ?? string.Empty,
                u.DrName ?? string.Empty,
                u.LoginId ?? string.Empty,
                u.LoginPass ?? string.Empty,
                u.MayakuLicenseNo ?? string.Empty,
                u.StartDate,
                u.EndDate,
                u.SortNo,
                u.RenkeiCd1 ?? string.Empty,
                u.IsDeleted);
        }
    }
}
