using MnbCurrencyReader.Entities;
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
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;

namespace MnbCurrencyReader
{
    public partial class Form1 : Form
    {
        BindingList<RateData> Rates = new BindingList<RateData>();
        BindingList<string> Currencies = new BindingList<string>();

        public Form1()
        {
            InitializeComponent();
            dataGridView1.DataSource = Rates;
            comboBox1.DataSource = Currencies;
            XMLProcessing2(CallWebService2());
            RefreshData();
        }
        private string CallWebService()
        {
            var mnbService = new MNBArfolyamServiceSoapClient(); // client access
            var request = new GetExchangeRatesRequestBody() // request parameters
            {
                currencyNames = comboBox1.Text,
                startDate = dateTimePicker1.Value.ToString(),
                endDate = dateTimePicker2.Value.ToString()
            };
                
            var response = mnbService.GetExchangeRates(request); // make the request
            var result = response.GetExchangeRatesResult; // store the result
            return result;
        }
        private string CallWebService2()
        {
            var mnbService = new MNBArfolyamServiceSoapClient(); // client access
            var request = new GetCurrenciesRequestBody(); // request parameters

            var response = mnbService.GetCurrencies(request); // make the request
            var result = response.GetCurrenciesResult; // store the result
            return result;
        }
        private void XMLProcessing(string result)
        {
            var xml = new XmlDocument();
            xml.LoadXml(result);
            foreach(XmlElement element in xml.DocumentElement)
            {
                var rate = new RateData();
                Rates.Add(rate);
                rate.Date = DateTime.Parse(element.GetAttribute("date")); // date
                var childElement = (XmlElement)element.ChildNodes[0];
                if(childElement == null)
                {
                    continue;
                }
                rate.Currency = childElement.GetAttribute("curr");
                var unit = decimal.Parse(childElement.GetAttribute("unit"));
                var value = decimal.Parse(childElement.InnerText);
                if(unit != 0)
                {
                    rate.Value = value / unit;
                }

            }
        }
        private void XMLProcessing2(string result)
        {
            var xml = new XmlDocument();
            xml.LoadXml(result);
            foreach (XmlElement element in xml.DocumentElement.ChildNodes[0])
            {
                var curr = element.InnerText;
                Currencies.Add(curr);
            }
        }
        private void CreateChart()
        {
            chartRateData.DataSource = Rates;
            var series = chartRateData.Series[0];
            series.ChartType = SeriesChartType.Line;
            series.XValueMember = "Date";
            series.YValueMembers = "Value";
            series.BorderWidth = 2;
            
            var legend = chartRateData.Legends[0];
            legend.Enabled = false;

            var chartArea = chartRateData.ChartAreas[0];
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisY.MajorGrid.Enabled = false;
            chartArea.AxisY.IsStartedFromZero = false;
            
        }
        private void RefreshData()
        {
            Rates.Clear();
            XMLProcessing(CallWebService());
            CreateChart();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshData();
        }
    }
}
