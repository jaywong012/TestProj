using Domain.Base;
using Domain.Common.Enums;

namespace Domain.Entities;

public class SocialAccessInfo : BaseModel
{
    public string? UserId { get; set; }

    public string UserName { get; set; }

    public string Token { get; set; }

    public string? AccessSecret { get; set; }

    public PostTypeEnum Type { get; set; }

    public Guid AccountId { get; set; }

    public Account Account { get; set; }

}