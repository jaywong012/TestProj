using NewProject.Services.Models;

namespace NewProject.Services.Interface;

public interface IXApiServices
{
    Task<bool> CheckUserRetweet(CheckRetweetRequest request);
}