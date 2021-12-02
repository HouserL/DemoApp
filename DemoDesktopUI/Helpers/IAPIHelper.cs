using DemoDesktopUI.Models;
using System.Threading.Tasks;

namespace DemoDesktopUI.Helpers
{
    public interface IAPIHelper
    {
        Task<AuthenticatedUser> Authenticate(string username, string password);
    }
}