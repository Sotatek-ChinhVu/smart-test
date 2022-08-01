﻿using Domain.Models.User;
using Entity.Tenant;
using Helper.Constant;
using Helper.Constants;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;
        public UserRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
        }

        public void Create(UserMstModel user)
        {
            throw new NotImplementedException();
        }

        public void Delete(int userId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserMstModel> GetAll()
        {
            return _tenantDataContext.UserMsts.Select(u => ConvertToModel(u)).ToList();
        }
        public IEnumerable<UserMstModel> GetAllDoctors()
        {
            var result = _tenantDataContext.UserMsts.Where(d => d.IsDeleted == 0 && d.JobCd == JobCdConstant.Doctor).ToList();
            return result.Select(u => ConvertToModel(u)).OrderBy(i => i.SortNo);
        }

        public UserMstModel? GetByUserId(int userId)
        {
            var entity = _tenantDataContext.UserMsts
                .Where(u => u.UserId == userId && u.IsDeleted == DeleteTypes.None).FirstOrDefault();
            return entity is null ? null : ConvertToModel(entity);
        }

        public int MaxUserId()
        {
            return _tenantDataContext.UserMsts.Max(u => u.UserId);
        }

        public UserMstModel Read(int userId)
        {
            throw new NotImplementedException();
        }

        public void Update(UserMstModel user)
        {
            throw new NotImplementedException();
        }

        private UserMstModel ConvertToModel(UserMst itemData)
        {
            return new UserMstModel(
                itemData.HpId,
                itemData.UserId,
                itemData.JobCd,
                itemData.ManagerKbn,
                itemData.KaId,
                itemData.KanaName ?? String.Empty,
                itemData.Name,
                itemData.Sname,
                itemData.LoginId,
                itemData.LoginPass,
                itemData.MayakuLicenseNo ?? String.Empty,
                itemData.StartDate,
                itemData.EndDate,
                itemData.SortNo,
                itemData.IsDeleted,
                itemData.RenkeiCd1 ?? String.Empty,
                itemData.DrName
              );
        }

    }
}
