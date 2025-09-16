namespace Domain.Base;

public class SocialAccessTokensInfo
{
    public XTokens XTokens { get; set; }

    public FacebookTokens FacebookTokens { get; set; }
}

public class XTokens
{
    public ConsumerToken ConsumerToken { get; set; }

    public string AccessToken { get; set; }
}

public class FacebookTokens
{
    public string AccessToken { get; set; }
}

public class ConsumerToken
{
    public string ApiKey { get; set; }
    public string SecretKey { get; set; }
}