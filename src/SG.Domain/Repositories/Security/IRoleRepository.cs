using SG.Domain.Base;
using SG.Domain.Entities.Security;

namespace SG.Domain.Repositories.Security;

public interface IRoleRepository : IBaseGenericRepository<Role>
{
    Task<bool> AddUsersToRole(int idRole, List<int> listIdUsers);
    Task<bool> AddUsersToRole(string codeRole, List<int> listIdUsers);
}
