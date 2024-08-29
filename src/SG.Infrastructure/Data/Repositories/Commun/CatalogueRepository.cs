using SG.Domain.Commun.Entities;
using SG.Domain.Commun.Repositories;
using SG.Infrastructure.Data.Context;

namespace SG.Infrastructure.Data.Repositories.Commun;

public class CatalogueRepository : BaseGenericRepository<Catalogue>, ICatalogueRepository
{
    public CatalogueRepository(ApplicationDbContext context) : base(context)
    {

    }
}
