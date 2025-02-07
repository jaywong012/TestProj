using Domain.Entities;
using Domain.Interfaces.IRepositories;
using Infrastructure.Configurations;

namespace Infrastructure.Repositories;

public class AccountPostShareRepository(NewProjectDbContext context) : GenericRepository<AccountPostShare>(context), IAccountPostShareRepository;