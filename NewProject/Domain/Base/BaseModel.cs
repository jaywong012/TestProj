namespace Domain.Base;

public class BaseModel
{
    public Guid Id { get; init; }

    public bool IsDeleted { get; set; }

    public DateTime LastCreatedTime { get; set; }

    public DateTime LastSavedTime { get; set; }
}