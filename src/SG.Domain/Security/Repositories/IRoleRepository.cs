using SG.Domain.Security.Entities;

namespace SG.Domain.Security.Repositories;

public interface IRoleRepository : IBaseGenericRepository<Role>
{
    Task<bool> AddUsersToRole(int idRole, List<int> listIdUsers);
    Task<bool> AddUsersToRole(string codeRole, List<int> listIdUsers);
}
