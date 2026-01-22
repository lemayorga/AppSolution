using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SG.Domain.Entities.Security;
using SG.Infrastructure.Data.Context;
using SG.Shared.Helpers;
using SG.Shared.Responses;
using SG.Shared.Settings;

namespace SG.Infrastructure.Data.Seeders;

public partial class SeederExecute
{
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _context;
    
    private SeederExecute(IConfiguration configuration, ApplicationDbContext context)
    {
        _configuration = configuration;
        _context = context;
    }
 
    public static async Task SeedAsync(ApplicationDbContext context, IConfiguration configuration, bool isTest = false)
    {
        SeederExecute seederExec = new(configuration, context);
        var settings = new AppConfiguration("appsettingsSeeders.json", Directory.GetCurrentDirectory());
        var seederApplication  = settings.GetSectionAsObject<DataApplicationSeedersSettings>(NamesApplicationSettings.DataApplicationSeeders);

        if(seederApplication.Execute || isTest)
        {
            await seederExec.InsertDataInitial(seederApplication);
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
             await InsertPasswordPolicies(seedersConfig.PasswordPolicies);
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
            if(!_context.Role.Any(y => y.CodeRol == appSettingRoles[i].Code))
            {
                var roleAdd =  new Role 
                {
                    CodeRol = appSettingRoles[i].Code, 
                    Name = appSettingRoles[i].Name, 
                    IsActive = true 
                };

                await _context.Role.AddAsync(roleAdd);
                countSave ++;
            }
        }

        if(countSave > 0) 
        {
            await  _context.SaveChangesAsync();
        }

        return _context.Role.AsQueryable();
    }

    private async Task CreateUsers(IQueryable<Role> roles, List<AppSettingUsers> appSettingUsers)
    {
        List<UsersRoles> userRolesAdd = new();
        for (int i = 0; i < appSettingUsers.Count(); i++)
        {
            var userConfig = appSettingUsers[i];
            User? user = await _context.User.FirstOrDefaultAsync(y => y.Email == userConfig.Email);

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

                await _context.User.AddAsync(user);
            }

             await AddUserToRole(roles.ToList(), userConfig!, user!);
        }

        await _context.SaveChangesAsync();
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
    
        if(await _context.UsersRoles.AnyAsync(f => f.IdRol == rol.Id && f.IdUser == user.Id && user.Id != 0))
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

    private async Task InsertPasswordPolicies(List<GeneralCodesValues> passwordPolicies)
    {

       var politiesExists =  _context.PasswordPolicy.Select(x => x.Code).ToArray();     
       var _passwordPolicies = passwordPolicies.Where(f => !politiesExists.Contains(f.Code))
                             .Select(x => new PasswordPolicy { Code = x.Code , Value = x.Value, Description = x.Description  });

       await  _context.PasswordPolicy.AddRangeAsync(_passwordPolicies);
       await  _context.SaveChangesAsync();
    }
}