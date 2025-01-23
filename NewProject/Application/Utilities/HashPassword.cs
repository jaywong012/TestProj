namespace Application.Utilities;

public static class HashPassword
{
    public static string Hash(string password)
    {

        var salt = BCrypt.Net.BCrypt.GenerateSalt();

        return BCrypt.Net.BCrypt.HashPassword(password, salt);
    }
}