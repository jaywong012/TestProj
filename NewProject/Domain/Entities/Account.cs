using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace Domain.Entities;

public class Account : BaseModel
{
    [MaxLength(100)]
    public required string UserName { get; init; }

    [MaxLength(100)]
    public required string Hash { get; init; }

    [MaxLength(20)]
    public required string Role { get; init; }
}