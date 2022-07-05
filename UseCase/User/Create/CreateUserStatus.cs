using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.User.Create
{
    public enum CreateUserStatus : byte
    {
        Success = 0,
        InvalidName = 1,
    }
}
