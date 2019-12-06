using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Models;

namespace TaskManager.Mapping
{
    public class UsersMap : ClassMapper<User>
    {
        public UsersMap()
        {
            Table("users");
            Map(x => x.Id).Column("id").Key(KeyType.Identity);
            Map(x => x.Email).Column("email");
            Map(x => x.Password).Column("password");
            Map(x => x.Username).Column("username");
        }
    }
}
