using Domain.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Product : BaseModel
{
    [MaxLength(100)]
    public required string Name { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    public Guid? CategoryId { get; set; }

    public Category? Category { get; init; }
}