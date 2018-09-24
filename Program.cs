using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Box.V2.Config;
using Box.V2.JWTAuth;
using Box.V2.Models;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Task t = MainAsync();
            t.Wait();

            Console.WriteLine();
            Console.Write("Press return to exit...");
            Console.ReadLine();
        }

        static async Task MainAsync()
        {
            IBoxConfig config = null;
            using (FileStream fs = new FileStream(@"S:\config.json", FileMode.Open))
            {
                config = BoxConfig.CreateFromJsonFile(fs);
            }

            var boxJWT = new BoxJWTAuth(config);

            var adminToken = boxJWT.AdminToken();
            var adminClient = boxJWT.AdminClient(adminToken);

            var userRequest = new BoxUserRequest() { Name = "testUser", IsPlatformAccessOnly = true };
            var appUser = await adminClient.UsersManager.CreateEnterpriseUserAsync(userRequest);

            var userToken = boxJWT.UserToken(appUser.Id);
            var userClient = boxJWT.UserClient(userToken, appUser.Id);

            var userDetails = await userClient.UsersManager.GetCurrentUserInformationAsync();
            Console.WriteLine("\nApp User Details:");
            Console.WriteLine("\tId: {0}", userDetails.Id);
            Console.WriteLine("\tName: {0}", userDetails.Name);
            Console.WriteLine("\tStatus: {0}", userDetails.Status);
            Console.WriteLine();


            var users = await adminClient.UsersManager.GetEnterpriseUsersAsync(autoPaginate: true);
            List<BoxUser> allBoxUsersList = users.Entries;

            // Display all users with name and id per row
            foreach (BoxUser boxUser in allBoxUsersList)
            {
                Console.WriteLine("Box User Name: {0} and Box Email: {1}", boxUser.Name, boxUser.Login);
            }

            string foundName = "";
            BoxUser found = new BoxUser();
            for (int i = 0; i < allBoxUsersList.Count; i++)
                if (allBoxUsersList[i].Login.Equals("test4@augusta-cyber.org"))
                {
                    foundName = allBoxUsersList[i].Name;
                    found = allBoxUsersList[i];
                    break;
                }
            Console.WriteLine(foundName);

            await adminClient.UsersManager.DeleteEnterpriseUserAsync(appUser.Id, false, true);

            Console.WriteLine("Deleted App User");

        }
    }
}

