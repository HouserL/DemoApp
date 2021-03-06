using DemoDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DemoDesktopUI.Library.API
{
    public class SaleEndPoint : ISaleEndPoint
    {
        private IAPIHelper _apiHelper;

        public SaleEndPoint(IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task PostSale(SaleModel sale)
        {
            using (HttpResponseMessage respone = await _apiHelper.ApiClient.PostAsJsonAsync("/api/Sale", sale))
            {
                if (respone.IsSuccessStatusCode)
                {
                    //Log succesful call?
                }
                else
                {
                    throw new Exception(respone.ReasonPhrase);
                }
            }
        }
    }
}
