using MnbCurrencyReader.MnbServiceReference;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MnbCurrencyReader
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            CallWebService();
        }
        private void CallWebService()
        {
            var mnbService = new MNBArfolyamServiceSoapClient(); // client access
            var request = new GetExchangeRatesRequestBody() // request parameters
            {
                currencyNames = "EUR",
                startDate = "2020-01-01",
                endDate = "2020-06-30"
            };
            var response = mnbService.GetExchangeRates(request); // make the request
            var result = response.GetExchangeRatesResult; // store the result
        }
    }
}
