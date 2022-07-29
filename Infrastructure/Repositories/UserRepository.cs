using Domain.Models.User;
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

        public void Create(UserMst user)
        {
            throw new NotImplementedException();
        }

        public void Delete(UserId userId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserMst> GetAll()
        {
            return _tenantDataContext.UserMsts.Select(u => new UserMst(u.UserId, u.Name)).ToList();
        }

        public int MaxUserId()
        {
            return _tenantDataContext.UserMsts.Max(u => u.UserId);
        }

        public UserMst Read(UserId userId)
        {
            throw new NotImplementedException();
        }

        public void Update(UserMst user)
        {
            throw new NotImplementedException();
        }

        public int GetUserIdBySname(string sname)
        {
            var record = _tenantDataContext.UserMsts.AsNoTracking()
                .Where(u => u.Sname == sname).Select(u => new { u.UserId }).FirstOrDefault();
            return record is null ? CommonConstants.InvalidId : record.UserId;
        }
    }
}
