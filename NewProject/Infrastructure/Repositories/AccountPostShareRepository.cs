using Domain.Entities;
using Domain.Interfaces.IRepositories;
using Infrastructure.Configurations;

namespace Infrastructure.Repositories;

public class PostRepository(NewProjectDbContext context) : GenericRepository<Post>(context), IPostRepository;