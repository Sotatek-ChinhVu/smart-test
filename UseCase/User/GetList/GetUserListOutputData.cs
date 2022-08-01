using Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.User.GetList
{
    public class GetUserListOutputData : IOutputData
    {
        public List<UserMstModel> UserList { get; private set; }

        public GetUserListOutputData(List<UserMstModel> userList)
        {
            UserList = userList;
        }
    }
}
