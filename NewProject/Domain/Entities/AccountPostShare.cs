using Domain.Base;

namespace Domain.Entities;

public class AccountPostShare : BaseModel
{
    public Guid AccountId { get; set; }

    public Guid PostId { get; set; }

    public Account Account { get; set; }

    public Post Post { get; set; }
}