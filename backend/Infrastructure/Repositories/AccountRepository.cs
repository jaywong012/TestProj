using Domain.Entities;
using Domain.Interfaces.IRepositories;
using Infrastructure.Configurations;

namespace Infrastructure.Repositories;

public class AccountRepository(NewProjectDbContext context) : GenericRepository<Account>(context), IAccountRepository;