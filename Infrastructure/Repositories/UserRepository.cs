using Domain.Models.User;
using Infrastructure.Constants;
using Infrastructure.Interfaces;
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
        public IEnumerable<UserMst> GetAllDoctors()
        {
            return _tenantDataContext.UserMsts.Where(d => d.IsDeleted == 0 && d.JobCd == JobCdConstant.Doctor).Select(u => new UserMst(u.UserId, u.Name)).OrderBy(i => i.SortNo).ToList();
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
    }
}
