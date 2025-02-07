namespace Domain.ErrorHandlingManagement;

public class TooManyRequestException(string message) : Exception(message);