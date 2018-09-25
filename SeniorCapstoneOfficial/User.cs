using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeniorCapstoneOfficial
{
    public class User
    {
        public string fName;
        public string lName;
        public string uname;
        public string password;
        public int admin;

        public User(string fName, string lName, string uname, string password, int admin)
        {
            this.fName = fName;
            this.lName = lName;
            this.uname = uname;
            this.password = password;
            this.admin = admin;
        }

        public User(string fName, string lName, string uname)
        {
            this.fName = fName;
            this.lName = lName;
            this.uname = uname;
        }

        
    }
}