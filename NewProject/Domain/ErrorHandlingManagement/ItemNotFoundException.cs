namespace Domain.ErrorHandlingManagement;

public class ItemNotFoundException : Exception
{
    public ItemNotFoundException(string message) : base(message)
    {
    }
}