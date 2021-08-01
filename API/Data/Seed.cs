using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using API.Entities;
using System.Security.Cryptography;
using System.Text;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext context) //return nothing hence no return type in the declaration
        {
            if(await context.Users.AnyAsync()) return; //check if user table contains any user. return if there is any user

            //if have no users in the database, populate the db with the users from the json file
            var userData= await File.ReadAllTextAsync("Data/UserSeedData.json");
            var users= JsonSerializer.Deserialize<List<AppUser>>(userData);

            foreach(var user in users)
            {
                using var hmac=new HMACSHA512();

                user.UserName=user.UserName.ToLower();
                user.PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes("Passw0rd"));
                user.PasswordSalt=hmac.Key;

                context.Users.Add(user);                
            }

            await context.SaveChangesAsync();            

        }
    }
}