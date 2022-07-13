using Domain.Models.User;
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
        private readonly TenantDataContext _tenantDataContext;
        public UserRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetDataContext();
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
            return _tenantDataContext.UserMsts.Select(u => new UserMst(u.Id, u.Name)).ToList();
        }

        public int MaxUserId()
        {
            return 100;
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
