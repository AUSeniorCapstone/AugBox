using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Box.V2;
using Box.V2.Config;
using Box.V2.JWTAuth;
using Box.V2.Models;
using System.Linq;

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


        public Box.V2.BoxClient Authenticate()
        {
            IBoxConfig config = null;
            using (FileStream fs = new FileStream(@"C:\Users\BirdHouse\AugBox\pkey.json", FileMode.Open))
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
            using (FileStream fs = new FileStream(@"C:\Users\BirdHouse\AugBox\pkey.json", FileMode.Open))
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
            IBoxConfig config = null;
            using (FileStream fs = new FileStream(@"C:\Users\BirdHouse\AugBox\pkey.json", FileMode.Open))
            {
                config = BoxConfig.CreateFromJsonFile(fs);
            }

            var boxJWT = new BoxJWTAuth(config);
            var adminToken = boxJWT.AdminToken();
            var adminClient = boxJWT.AdminClient(adminToken);

            BoxCollection<BoxEmailAlias> aliases = await adminClient.UsersManager.GetEmailAliasesAsync(ID);
            return aliases;
        }

        public async Task<BoxEmailAlias> CreateEmailAlias(String ID, String email)
        {
            IBoxConfig config = null;
            using (FileStream fs = new FileStream(@"C:\Users\BirdHouse\AugBox\pkey.json", FileMode.Open))
            {
                config = BoxConfig.CreateFromJsonFile(fs);
            }

            var boxJWT = new BoxJWTAuth(config);
            var adminToken = boxJWT.AdminToken();
            var adminClient = boxJWT.AdminClient(adminToken);

            BoxEmailAlias alias = await adminClient.UsersManager.AddEmailAliasAsync(ID, email);
            return alias;
        }

        public async Task DeleteEmailAlias(String ID, String emailID)
        {
            IBoxConfig config = null;
            using (FileStream fs = new FileStream(@"C:\Users\BirdHouse\AugBox\pkey.json", FileMode.Open))
            {
                config = BoxConfig.CreateFromJsonFile(fs);
            }

            var boxJWT = new BoxJWTAuth(config);
            var adminToken = boxJWT.AdminToken();
            var adminClient = boxJWT.AdminClient(adminToken);

            await adminClient.UsersManager.DeleteEmailAliasAsync(ID, emailID);
        }
    }
}