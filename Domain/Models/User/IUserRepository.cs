using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.User
{
    public interface IUserRepository
    {
        void Create(UserMst user);

        UserMst Read(UserId userId);

        void Update(UserMst user);

        void Delete(UserId userId);

        IEnumerable<UserMst> GetAll();

        int MaxUserId();
        int GetUserIdBySname(string sname);
    }
}
