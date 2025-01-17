using Domain.Entities;
using Domain.Interfaces.IRepositories;
using Infrastructure.Configurations;

namespace Infrastructure.Repositories;

public class AccountRepository : GenericRepository<Account>, IAccountRepository
{
    public AccountRepository(NewProjectDbContext context) : base(context) { }
}