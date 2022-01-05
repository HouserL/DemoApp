using DemoDesktopUI.Library.Models;
using System.Threading.Tasks;

namespace DemoDesktopUI.Library.API
{
    public interface ISaleEndPoint
    {
        Task PostSale(SaleModel sale);
    }
}