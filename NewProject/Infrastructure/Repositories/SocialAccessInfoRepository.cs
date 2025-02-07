using Domain.Entities;
using Domain.Interfaces.IRepositories;
using Infrastructure.Configurations;

namespace Infrastructure.Repositories;

public class SocialAccessInfoRepository(NewProjectDbContext context) : GenericRepository<SocialAccessInfo>(context), ISocialAccessInfoRepository;