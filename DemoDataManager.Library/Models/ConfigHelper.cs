using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDataManager.Library.Models
{
    public class ConfigHelper
    {
        // TODO: Move this from config to the API
        public static decimal GetHighTaxRate()
        {
            string rateText = ConfigurationManager.AppSettings["HighTaxRate"];
            bool isValidTaxRate = Decimal.TryParse(rateText, out decimal output);

            if (isValidTaxRate == false)
            {
                throw new ConfigurationErrorsException("The high tax rate is not set up properly");
            }

            return output;
        }

        public static decimal GetLowTaxRate()
        {
            string rateText = ConfigurationManager.AppSettings["LowTaxRate"];
            bool isValidTaxRate = Decimal.TryParse(rateText, out decimal output);

            if (isValidTaxRate == false)
            {
                throw new ConfigurationErrorsException("The low tax rate is not set up properly");
            }

            return output;
        }
    }
}
