using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace Domain.Entities;

public class Account : BaseModel
{
    [MaxLength(100)]
    public required string UserName { get; set; }

    [MaxLength(100)]
    public required string Hash { get; set; }

    [MaxLength(50)]
    public required string Salt { get; set; }

    [MaxLength(20)]
    public required string Role { get; set; }
}