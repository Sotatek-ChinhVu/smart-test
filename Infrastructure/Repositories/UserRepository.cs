using Domain.Models.User;
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
        private TenantDataContext _tenantDataContext;

        public void Create(User user)
        {
            _tenantDataContext = new TenantDataContext();
        }

        public void Delete(UserId userId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAll()
        {
            var users = _tenantDataContext.UserMsts.Where(u => u.IsDeleted == 0).Select(u => u.Name).ToList();

            return new List<User>();
        }

        public int MaxUserId()
        {
            return 100;
        }

        public User Read(UserId userId)
        {
            throw new NotImplementedException();
        }

        public void Update(User user)
        {
            throw new NotImplementedException();
        }
    }
}
