using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.User;

namespace Domain.Models.User
{
    public class UserMst
    {
        public UserId Id { get; private set; }

        public Name Name { get; private set; }

        public UserMst(UserId id, Name name)
        {
            Id = id;
            Name = name;
        }

        public UserMst(long id, string name)
        {
            Id = UserId.From(id);
            Name = Name.From(name);
        }
    }
}
