using NewProject.Services.Models;

namespace NewProject.Services.Interface;

public interface IFacebookApiService
{
    Task<bool> CheckFacebookUserRetweet(CheckRetweetRequest request);

    Task<string> RetrieveUrlByTitle(string title);
}