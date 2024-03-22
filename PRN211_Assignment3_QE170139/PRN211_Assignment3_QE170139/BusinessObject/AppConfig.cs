using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class AppConfig
    {
        public DefaultAccount DefaultAccount { get; set; }
    }

    public class DefaultAccount
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
