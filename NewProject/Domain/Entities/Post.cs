using System.ComponentModel.DataAnnotations.Schema;
using Domain.Base;
using Domain.Common.Enums;

namespace Domain.Entities;

public class Post : BaseModel
{
    public string Url { get; set; }

    public PostTypeEnum Type { get; set; }

    [Column(TypeName = "varchar(max)")]
    public string? RetweetedUsers { get; set; }

    public ICollection<AccountPostShare>? AccountPostShares { get; init; }
}