using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Box.V2;
using Box.V2.Config;
using Box.V2.JWTAuth;
using Box.V2.Models;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SeniorCapstoneOfficial
{
    public class BoxAuthTest
    {
        public async Task<List<BoxUser>> GetallUsers()
        {

            var adminClient = Authenticate();
            //limit is 1000, do a loop
            var users = await adminClient.UsersManager.GetEnterpriseUsersAsync(autoPaginate: true);

            List<BoxUser> allBoxUsersList = users.Entries;

            return allBoxUsersList;
        }
        public async Task<string> GetLogins()
        {
            IBoxConfig config = null;
            using (FileStream fs = new FileStream(@"C:\Users\Gabriel\Documents\AugBoxApplication\pkey.json", FileMode.Open))
            {
                config = BoxConfig.CreateFromJsonFile(fs);
            }

            var boxJWT = new BoxJWTAuth(config);
            var adminToken = boxJWT.AdminToken();

            HttpWebRequest req = WebRequest.Create("https://api.box.com/2.0/events?stream_type=admin_logs&limit=500&event_type=LOGIN") as HttpWebRequest;
            req.Headers.Add("Authorization: Bearer " + adminToken);
            string result = null;
            using (HttpWebResponse resp = req.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(resp.GetResponseStream());
                result = reader.ReadToEnd();
            }
            string latest = result;
           
            return latest;
        }

        public Box.V2.BoxClient Authenticate()
        {
            IBoxConfig config = null;
            using (FileStream fs = new FileStream(@"C:\Users\Gabriel\Documents\AugBoxApplication\pkey.json", FileMode.Open))
            {
                config = BoxConfig.CreateFromJsonFile(fs);
            }

            var boxJWT = new BoxJWTAuth(config);

            var adminToken = boxJWT.AdminToken();
            var adminClient = boxJWT.AdminClient(adminToken);

            return adminClient;

        }

        public async Task<List<BoxItem>> GetFolder(String ID)
        {
            IBoxConfig config = null;
            using (FileStream fs = new FileStream(@"C:\Users\Gabriel\Documents\AugBoxApplication\pkey.json", FileMode.Open))
            {
                config = BoxConfig.CreateFromJsonFile(fs);
            }

            var boxJWT = new BoxJWTAuth(config);
            var adminToken = boxJWT.AdminToken();
            var adminClient = boxJWT.AdminClient(adminToken);
            //List<BoxUser> allBoxUsersList = boxUsers.Entries;
            // var userRequest = new BoxUserRequest() { Name = "test appuser", IsPlatformAccessOnly = true };
            // var appUser = await adminClient.UsersManager.CreateEnterpriseUserAsync(userRequest);

            var userToken = boxJWT.UserToken(ID);
            var userClient = boxJWT.UserClient(userToken, ID);
            var boxFolderItems = await userClient.FoldersManager.GetFolderItemsAsync("0", 100);
            List<BoxItem> boxFolderItemsList = boxFolderItems.Entries;
            return boxFolderItemsList;

        }

        public async Task<BoxCollection<BoxEmailAlias>> GetEmailAlias(String ID)
        {
            var adminClient = Authenticate();

            BoxCollection<BoxEmailAlias> aliases = await adminClient.UsersManager.GetEmailAliasesAsync(ID);
            return aliases;
        }

        public async Task<BoxEmailAlias> CreateEmailAlias(String ID, String email)
        {
            var adminClient = Authenticate();

            BoxEmailAlias alias = await adminClient.UsersManager.AddEmailAliasAsync(ID, email);
            return alias;
        }

        public async Task DeleteEmailAlias(String ID, String emailID)
        {
            var adminClient = Authenticate();

            await adminClient.UsersManager.DeleteEmailAliasAsync(ID, emailID);
        }

        public List<string> GetRecentEvents(String ID)
        {
            IBoxConfig config = null;
            using (FileStream fs = new FileStream(@"C:\Users\Gabriel\Documents\AugBoxApplication\pkey.json", FileMode.Open))
            {
                config = BoxConfig.CreateFromJsonFile(fs);
            }

            var boxJWT = new BoxJWTAuth(config);
            var adminToken = boxJWT.AdminToken();
            var adminClient = boxJWT.AdminClient(adminToken);

            var userToken = boxJWT.UserToken(ID);
            var userClient = boxJWT.UserClient(userToken, ID);

            HttpWebRequest req = WebRequest.Create("https://api.box.com/2.0/events") as HttpWebRequest;
            req.Headers.Add("Authorization: Bearer " + userToken);
            string result = null;
            using (HttpWebResponse resp = req.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(resp.GetResponseStream());
                result = reader.ReadToEnd();
            }
            string[] latest = result.Split(new string[] { "}}," }, StringSplitOptions.RemoveEmptyEntries);
            List<string> alist = new List<string>();
            for (int i = latest.Length - 1; i >= 0; i--)
            {
                if (latest[i] != null && alist.Count <= 10)
                {
                    alist.Add(latest[i]);
                }

                else
                {
                    break;
                }
            }
            // latest.Reverse().Take(10);
            return alist;

        }
    }

}
