using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Box.V2;
using Box.V2.Config;
using Box.V2.JWTAuth;
using Box.V2.Models;

namespace SeniorCapstoneOfficial
{
    public class BoxAuthTest
    {
        public async Task<List<BoxUser>> GetallUsers()
        {

            var adminClient = await Authenticate();
            //limit is 1000, do a loop
            var users = await adminClient.UsersManager.GetEnterpriseUsersAsync(autoPaginate: true);

            List<BoxUser> allBoxUsersList = users.Entries;

            return allBoxUsersList;

        }


        public async Task<Box.V2.BoxClient> Authenticate()
        {
            IBoxConfig config = null;
<<<<<<< HEAD
            using (FileStream fs = new FileStream(@"E:\config.json", FileMode.Open))
=======
            using (FileStream fs = new FileStream(@"C:\Users\Gabriel\Documents\AugBoxPROJECT\pkey.json", FileMode.Open))
>>>>>>> 730cbe4b329d8c05d25f7339b341147a846e3332
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
<<<<<<< HEAD
            using (FileStream fs = new FileStream(@"E:\config.json", FileMode.Open))
=======
            using (FileStream fs = new FileStream(@"C:\Users\Gabriel\Documents\AugBoxPROJECT\pkey.json", FileMode.Open))
>>>>>>> 730cbe4b329d8c05d25f7339b341147a846e3332
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
    }
}