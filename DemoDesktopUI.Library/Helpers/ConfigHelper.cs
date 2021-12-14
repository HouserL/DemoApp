using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDesktopUI.Library.Helpers
{
    public class ConfigHelper : IConfigHelper
    {
        public decimal GetHighTaxRate()
        {
            string rateText = ConfigurationManager.AppSettings["HighTaxRate"];
            bool isValidTaxRate = Decimal.TryParse(rateText, out decimal output);

            if (isValidTaxRate == false)
            {
                throw new ConfigurationErrorsException("The high tax rate is not set up properly");
            }

            return output;
        }

        public decimal GetLowTaxRate()
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
