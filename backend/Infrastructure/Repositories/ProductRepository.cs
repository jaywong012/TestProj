using Domain.Entities;
using Domain.Interfaces.IRepositories;
using Infrastructure.Configurations;

namespace Infrastructure.Repositories;

public class ProductRepository(NewProjectDbContext context) : GenericRepository<Product>(context), IProductRepository;