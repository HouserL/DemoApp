using DemoDataManager.Library.DataAccess;
using DemoDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DemoDataManager.Controllers
{
    //[Authorize]
    public class ProductController : ApiController
    {
        [HttpGet]
        public List<ProductModel> Get()
        {
            ProductData data = new ProductData();

            return data.GetProducts();
        }
    }
}
