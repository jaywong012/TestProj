namespace Infrastructure.Utilities;

public static class FacebookUrlParser
{
    public static string ExtractPostId(string url)
    {
        if (string.IsNullOrEmpty(url))
            throw new ArgumentException("URL cannot be empty");

        Uri uri = new Uri(url);
        string[] segments = uri.AbsolutePath.Split('/');

        if (segments.Length >= 3 && segments[1] == "share" && segments[2] == "p")
        {
            return segments[3]; // This should be the post ID
        }

        throw new Exception("Invalid Facebook share URL format");
    }
}