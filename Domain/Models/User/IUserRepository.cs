using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.User
{
    public interface IUserRepository
    {
        void Create(UserMstModel user);

        UserMstModel Read(UserId userId);

        void Update(UserMstModel user);

        void Delete(UserId userId);

        List<UserMstModel> GetAll(int sinDate, bool isDoctorOnly);

        int MaxUserId();
        UserMstModel? GetByUserId(int userId);
    }
}
