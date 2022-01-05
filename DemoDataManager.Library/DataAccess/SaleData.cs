using DemoDataManager.Library.Internal.DataAccess;
using DemoDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDataManager.Library.DataAccess
{
    public class SaleData
    {
       

        public void SaveSale(SaleModel saleInfo, string cashierId)
        {
            //TODO: Remove parts of this and put in logic
            // Start filling in the save detail models we will save to the DB
            List<SaleDetailDBModel> saleDetails = new List<SaleDetailDBModel>();
            ProductData products = new ProductData();
            var highTaxRate = ConfigHelper.GetHighTaxRate();
            var lowTaxRate = ConfigHelper.GetLowTaxRate();  
            foreach (var item in saleInfo.SaleDetails)
            {
                var detail = new SaleDetailDBModel
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantitiy
                };

                //Get information about product
                var productInfo = products.GetProductById(item.ProductId);
                
                if (productInfo == null)
                {
                    throw new Exception($"The product Id of { item.ProductId } could not be found in the database");
                }
                detail.PurchasePrice = productInfo.RetailPrice * detail.Quantity;

                if (productInfo.IsHighTaxable)
                {
                    detail.Tax = (detail.Tax * highTaxRate / 100);
                }
                else if (productInfo.IsLowTaxable)
                {
                    detail.Tax = (detail.Tax * lowTaxRate / 100);
                }

                saleDetails.Add(detail);
            }

            // Fill in avaliable information
            // Create the Sale Model
            SaleDBModel sale = new SaleDBModel
            {
                SubTotal = saleDetails.Sum(x => x.PurchasePrice),
                Tax = saleDetails.Sum(x => x.Tax),
                CashierId = cashierId
            };

            sale.Total = sale.SubTotal + sale.Tax;

            // Save the sale Model

            SQLDataAccess sql = new SQLDataAccess();
            sql.SaveData("dbo.spSale_Insert", sale, "DemoData");

            // Get the ID for the sale model 


            // Finish filling in the sale detail models
            foreach (var item in saleDetails)
            {
                item.SaleId = sale.Id;
                // Save the sale detail models.
                sql.SaveData("dbo.spSaleDetail_Insert", item, "DemoData");
            }

        }
        //public List<ProductModel> GetProducts()
        //{
        //    SQLDataAccess sql = new SQLDataAccess();

        //    var output = sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll", new { }, "DemoData");

        //    return output;
        //}
    }
}
