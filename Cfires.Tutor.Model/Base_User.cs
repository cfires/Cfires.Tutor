using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cfires.Tutor.Model
{
    [TableName("Base_User")]
    [PrimaryKey("Id")]
    public class Base_User
    {
        [Column("Id")]
        public int Id { get; set; }

        [Column("Type")]
        public UserType Type { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("Password")]
        public string Password { get; set; }

        [Column("Email")]
        public string Email { get; set; }

        [Column("CreateDate")]
        public DateTime CreateDate { get; set; }
    }
}
