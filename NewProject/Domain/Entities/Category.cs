using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace Domain.Entities;

public class Category : BaseModel
{
    [Required]
    public required string Name { get; set; }

    public ICollection<Product>? Products { get; set; }
}