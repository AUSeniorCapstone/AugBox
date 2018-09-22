using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeniorCapstoneOfficial
{
    public class User
    {
        public string uname;
        public string password;
        public int admin;

        public User(string uname, string password, int admin)
        {
            this.uname = uname;
            this.password = password;
            this.admin = admin;
        }
    }
}