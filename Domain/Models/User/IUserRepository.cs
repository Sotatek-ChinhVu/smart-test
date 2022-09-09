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

        UserMstModel Read(int userId);

        void Update(UserMstModel user);

        void Delete(int userId);

        IEnumerable<UserMstModel> GetAll();

        IEnumerable<UserMstModel> GetDoctorsList(int userId);

        List<UserMstModel> GetAll(int sinDate, bool isDoctorOnly);

        int MaxUserId();

        UserMstModel? GetByUserId(int userId);

        UserMstModel? GetByLoginId(string loginId);

        bool CheckExistedId(List<long> idList);

        void Upsert(List<UserMstModel> updatedUserList, List<UserMstModel> inserteddUserList);
    }
}
