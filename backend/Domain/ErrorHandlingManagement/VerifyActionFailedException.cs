namespace Domain.ErrorHandlingManagement;

public class VerifyActionFailedException(string message) : Exception(message);