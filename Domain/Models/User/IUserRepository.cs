using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.User
{
    public interface IUserRepository
    {
        void Create(User user);

        User Read(UserId userId);

        void Update(User user);

        void Delete(UserId userId);

        IEnumerable<User> GetAll();

        int MaxUserId();
    }
}
