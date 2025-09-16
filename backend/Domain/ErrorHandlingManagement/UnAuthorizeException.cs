namespace Domain.ErrorHandlingManagement;

public class UnAuthorizeException(string message) : Exception(message);