using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SG.Domain.Security.Entities;
using SG.Infrastructure.Data.Context;
using SG.Shared.Helpers;

namespace SG.Infrastructure.Data.Seeders;


public static class SeederExecute
{
    public static async Task SeedAsync(ApplicationDbContext context, IConfiguration configuration)
    {
        DataApplication seedersConfig = new();
        configuration.GetSection("DataApplicationSeeders").Bind(seedersConfig);

        if(seedersConfig.Execute)
        {
            await InsertDataInitial(context, seedersConfig);
        }
    }

    private static async Task InsertDataInitial(ApplicationDbContext context, DataApplication seedersConfig)
    {
        try
        {
            var roles = new List<Role>()
            {
                new Role {CodeRol = "SADMIN", Name = "Super Admin", IsActive = true },
                new Role {CodeRol = "ADMIN", Name = "Admin", IsActive = true },
                new Role {CodeRol = "SALER", Name = "Saler", IsActive = true },
                new Role {CodeRol = "MANAGER", Name = "Manager", IsActive = true },            
            }; 


            var users = new List<User>() 
            {
                new User{Username= "sadmin", Email="sadmin@demo.com", Firstname= "sadmin", Lastname= "sadmin", Password ="", IsActive = true, IsLocked = false }
            };

            for (int i = 0; i < roles.Count(); i++)
            {
                if(!context.Role.Any(y => y.CodeRol == roles[i].CodeRol))
                {
                    await context.Role.AddAsync(roles[i]);
                }
            }


            if(seedersConfig.Users.Any())
            {
                for (int i = 0; i < seedersConfig.Users.Count(); i++)
                {
                    var userConfig = seedersConfig.Users[i];
                    if(!context.User.Any(y => y.Email == userConfig.Email))
                    {
                        var user = new User
                        {
                            Username= userConfig.UserName, 
                            Email= userConfig.Email, 
                            Firstname=  userConfig.UserName, 
                            Lastname= userConfig.UserName, 
                            Password = EncryptionUtils.HashText(userConfig.Password), 
                            IsActive = true, 
                            IsLocked = false 
                        };

                        await context.User.AddAsync(user);
                    }
                }

               await  context.SaveChangesAsync();

                for (int i = 0; i < seedersConfig.Users.Count(); i++)
                {
                    var userConfig = seedersConfig.Users[i];
                    if(!string.IsNullOrWhiteSpace(userConfig?.CodeRol))
                    {
                        var rol = await context.Role.FirstOrDefaultAsync(x => x.CodeRol == userConfig.CodeRol);
                        var user = await context.User.FirstOrDefaultAsync(x => x.Username == userConfig.UserName);
                        if(user != null && rol != null)
                        {
                            context.UsersRoles.Add(new UsersRoles
                            {
                                IdUser = user.Id,
                                IdRol = rol.Id,
                                State = true
                            });
                        }
                    }
                }                
            }

           await  context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _ = ex;
        }
    }

    internal class DataApplication
    {
        public bool Execute { get; set; }
        public List<AppSettingUsuarios>? Users { get; set; }
        public Dictionary<string, bool>? FilesExecute { get; set; }
    }
    internal class AppSettingUsuarios
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string CodeRol { get; set; }
    }    
}