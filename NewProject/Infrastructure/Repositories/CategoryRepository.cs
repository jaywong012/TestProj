using Domain.Entities;
using Domain.Interfaces.IRepositories;
using Infrastructure.Configurations;

namespace Infrastructure.Repositories;

public class CategoryRepository(NewProjectDbContext context) : GenericRepository<Category>(context), ICategoryRepository;