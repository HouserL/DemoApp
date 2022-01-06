using DemoDataManager.Library.Internal.DataAccess;
using DemoDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDataManager.Library.DataAccess
{
    public class ProductData
    {
        public List<ProductModel> GetProducts()
        {
            SQLDataAccess sql = new SQLDataAccess();

            var output = sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll", new { }, "DemoData");

            return output;
        }

        public ProductModel GetProductById(int productId)
        {
            SQLDataAccess sql = new SQLDataAccess();

            var output = sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetById", new { Id = productId } , "DemoData")
                            .FirstOrDefault();

            return output;
        }

    }
}
