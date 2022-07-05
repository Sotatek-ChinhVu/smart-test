using Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.User.Create
{
    public class CreateUserInputData : IInputData<CreateUserOutputData>
    {
        public Name Name { get; private set; }

        public CreateUserInputData(string name)
        {
            Name = Name.From(name);
        }

        public Domain.Models.User.User GenerateUserModel(int id)
        {
            return new Domain.Models.User.User(UserId.From(id), Name);
        }
    }
}
