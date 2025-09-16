namespace Domain.ErrorHandlingManagement;

public class ItemNotFoundException(string message) : Exception(message);