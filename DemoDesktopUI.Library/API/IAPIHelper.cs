using DemoDesktopUI.Library.Models;
using System.Threading.Tasks;

namespace DemoDesktopUI.Library.API
{
    public interface IAPIHelper
    {
        Task<AuthenticatedUser> Authenticate(string username, string password);
        Task GetLoggedInUserInfo(string token);
    }
}