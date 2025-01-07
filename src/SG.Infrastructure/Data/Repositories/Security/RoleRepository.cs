using Microsoft.EntityFrameworkCore;
using SG.Domain.Security.Entities;
using SG.Domain.Security.Repositories;
using SG.Infrastructure.Data.Context;

namespace SG.Infrastructure.Data.Repositories.Security;

public class RoleRepository : BaseGenericRepository<Role>, IRoleRepository
{
    public RoleRepository(ApplicationDbContext context) : base(context)
    {

    }
    public async Task<bool> AddUsersToRole(string codeRole, List<int> listIdUsers)
    {
        if(string.IsNullOrWhiteSpace(codeRole))
        {
            return false;   
        }

        codeRole =  codeRole.Trim();
        var rol = await _context.Role.FirstOrDefaultAsync(x => x.CodeRol == codeRole);
        return await AddUsersToRole(rol, listIdUsers); 
    }
    
    public async Task<bool> AddUsersToRole(int idRole, List<int> listIdUsers)
    {
        var rol = await _context.Role.FirstOrDefaultAsync(x => x.Id == idRole);  
        return await AddUsersToRole(rol, listIdUsers); 
    }

    async Task<bool> AddUsersToRole(Role? rol, List<int> listIdUsers)
    {
        if(rol is null)
        {
            return false;
        }
            
        int idRole =  rol.Id;
        var listIdsUserRolesExisting = _context.UsersRoles.Where(x => x.IdRol == idRole && listIdUsers.Contains(x.IdUser))
                                        .Select(s => s.IdUser);

        var listUsersToSave = listIdUsers.Where(idUser => !listIdsUserRolesExisting.Contains(idUser))
                                .Select(idUser => new UsersRoles
                                {
                                    IdUser = idUser,  
                                    IdRol = idRole,  
                                    State = true
                                }).ToHashSet();

        await _context.UsersRoles.AddRangeAsync(listUsersToSave);
        return await _context.SaveChangesAsync() > 0; 
    }
}
