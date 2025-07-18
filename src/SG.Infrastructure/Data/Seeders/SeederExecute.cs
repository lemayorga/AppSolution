using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SG.Domain.Entities.Security;
using SG.Infrastructure.Data.Context;
using SG.Shared.Helpers;
using SG.Shared.Settings;

namespace SG.Infrastructure.Data.Seeders;

public partial class SeederExecute
{
    private readonly IConfiguration configuration;
    private ApplicationDbContext context;
    
    private SeederExecute(IConfiguration _configuration, ApplicationDbContext _context)
    {
        configuration = _configuration;
        context = _context;
    }
 
    public static async Task SeedAsync(ApplicationDbContext context, IConfiguration configuration)
    {
        SeederExecute seederExec = new(configuration, context);
        DataApplicationSeedersSettings seedersConfig = new();
        
        seederExec.configuration.GetSection(NamesApplicationSettings.DataApplicationSeeders).Bind(seedersConfig);

        if(seedersConfig.Execute)
        {
            await seederExec.InsertDataInitial(seedersConfig);
        }
    }

    private async Task InsertDataInitial(DataApplicationSeedersSettings seedersConfig)
    {
        try
        {
            var roles = await CreateRoles(seedersConfig.Roles);
            if (roles.Any())
            {
                await CreateUsers(roles, seedersConfig.Users);
            }
        }
        catch (Exception ex)
        {
            _ = ex;
        }
    }

    private async Task<IQueryable<Role>> CreateRoles(List<AppSettingRoles> appSettingRoles )
    {

        int countSave = 0;
        for (int i = 0; i < appSettingRoles.Count(); i++)
        {
            if(!context.Role.Any(y => y.CodeRol == appSettingRoles[i].Code))
            {
                var roleAdd =  new Role 
                {
                    CodeRol = appSettingRoles[i].Code, 
                    Name = appSettingRoles[i].Name, 
                    IsActive = true 
                };

                await context.Role.AddAsync(roleAdd);
                countSave ++;
            }
        }

        if(countSave > 0) 
        {
            await  context.SaveChangesAsync();
        }

        return context.Role.AsQueryable();
    }

    private async Task CreateUsers(IQueryable<Role> roles, List<AppSettingUsers> appSettingUsers)
    {
        List<UsersRoles> userRolesAdd = new();
        for (int i = 0; i < appSettingUsers.Count(); i++)
        {
            var userConfig = appSettingUsers[i];
            User? user = await context.User.FirstOrDefaultAsync(y => y.Email == userConfig.Email);

            if(user is null && userConfig is not null)
            {
                user = new User
                {
                    Username = userConfig.UserName, 
                    Email = userConfig.Email, 
                    Firstname =  userConfig?.Firstname ?? "", 
                    Lastname= userConfig?.Lastname ?? "", 
                    Password = EncryptionUtils.HashText(userConfig!.Password), 
                    IsActive = true, 
                    IsLocked = false 
                };

                await context.User.AddAsync(user);
            }

             await AddUserToRole(roles.ToList(), userConfig!, user!);
        }

        await context.SaveChangesAsync();
    }

    private async Task AddUserToRole(List<Role> listRoles, AppSettingUsers appSettingUser, User user)
    {
        if(string.IsNullOrWhiteSpace(appSettingUser?.CodeRol))
        {
            return;
        }

        var rol =  listRoles.FirstOrDefault(x => x.CodeRol == appSettingUser.CodeRol);
        if(rol is null)
        {
            return;
        }
    
        if(await context.UsersRoles.AnyAsync(f => f.IdRol == rol.Id && f.IdUser == user.Id && user.Id != 0))
        {
             return;
        }
        
        user.UsersRoles = new HashSet<UsersRoles>()
        {
            new UsersRoles
            {
                IdUser = user.Id,  
                IdRol = rol.Id,  
                State = true
            }
        }; 
    }
}