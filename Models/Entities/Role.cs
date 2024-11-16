using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class Role
    {
        public int ID { get; set; }
        public string RoleName { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
