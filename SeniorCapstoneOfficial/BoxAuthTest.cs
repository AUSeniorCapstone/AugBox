using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
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
            using (FileStream fs = new FileStream(@"C:\Users\Gabriel\Downloads\678301__config.json", FileMode.Open))
            {
                config = BoxConfig.CreateFromJsonFile(fs);
            }

            var boxJWT = new BoxJWTAuth(config);

            var adminToken = boxJWT.AdminToken();
            var adminClient = boxJWT.AdminClient(adminToken);

            return adminClient;

        }

        public async Task<BoxCollection<BoxItem>> GetFolder()
        {
            var adminClient = await Authenticate();
            var folders = await adminClient.FoldersManager.GetFolderItemsAsync("0", 1000);

            return folders;


        }
    }
}