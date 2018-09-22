using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SQLite;
using System.IO;
using System.Data;
using System.Security.Cryptography;

namespace SeniorCapstoneOfficial
{
    public class LoginControl
    {
        public int submit(string uname, string password)
        {
            DateTime dt = DateTime.Now;
            DBConnector newGet = new DBConnector();

            User user = newGet.getUser(uname);

            if (user.uname != null && user.password != null)
            {

                string savedPasswordHash = user.password;

                byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);

                byte[] salt = new byte[16];
                Array.Copy(hashBytes, 0, salt, 0, 16);

                var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
                byte[] hash = pbkdf2.GetBytes(20);

                for (int i = 0; i < 20; i++)
                    if (hashBytes[i + 16] != hash[i])
                        return 3;

                if (user.admin == 1)
                {
                    newGet.saveLogin(uname, dt);
                    return 1;
                }
                else if (user.admin == 0)
                {
                    newGet.saveLogin(uname, dt);
                    return 2;
                }
                return 3;
            }
            return 3;
        }
    }
}