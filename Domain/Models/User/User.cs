using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.User;

namespace Domain.Models.User
{
    public class User
    {
        public UserId Id { get; private set; }

        public Name Name { get; private set; }

        public User(UserId id, Name name)
        {
            Id = id;
            Name = name;
        }

        public User(long id, string name)
        {
            Id = UserId.From(id);
            Name = Name.From(name);
        }
    }
}
