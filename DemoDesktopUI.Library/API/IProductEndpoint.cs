using DemoDesktopUI.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoDesktopUI.Library.API
{
    public interface IProductEndpoint
    {
        Task<List<ProductModel>> GetAll();
    }
}