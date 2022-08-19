using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.User.Create
{
    public class CreateUserOutputData : IOutputData
    {
        public int UserId { get; private set; }

        public CreateUserStatus Status { get; private set; }

        public CreateUserOutputData(int userId, CreateUserStatus status)
        {
            UserId = userId;
            Status = status;
        }
    }
}
