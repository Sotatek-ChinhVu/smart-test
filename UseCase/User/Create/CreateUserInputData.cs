﻿using Domain.Models.User;
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

        public UserMst GenerateUserModel(int id)
        {
            return new UserMst(UserId.From(id), Name);
        }
    }
}
