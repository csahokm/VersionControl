using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ValueAtRiskAnalysis.Entities;

namespace ValueAtRiskAnalysis
{
    public partial class Form1 : Form
    {
        List<Tick> ticks;
        PortfolioEntities context = new PortfolioEntities();
        List<PortfolioItem> portfolio = new List<PortfolioItem>();

        public Form1()
        {
            InitializeComponent();
            ticks = context.Ticks.ToList();
            dataGridView1.DataSource = ticks;

            CreatePortfolio();

            List<decimal> gains = new List<decimal>();
            int interval = 30;
            DateTime startDate = (from x in ticks select x.TradingDay).Min();
            DateTime endDate = new DateTime(2016, 12, 30);
            TimeSpan z = endDate - startDate;
            for (int i = 0; i < z.Days - interval; i++)
            {
                decimal ny = GetPortfolioValue(startDate.AddDays(i + interval))
                           - GetPortfolioValue(startDate.AddDays(i));
                gains.Add(ny);
                Console.WriteLine(i + " " + ny);
            }

            var gainsOrdered = (from x in gains
                                      orderby x
                                      select x)
                                        .ToList();
            MessageBox.Show(gainsOrdered[gainsOrdered.Count() / 5].ToString());
        }
        private void CreatePortfolio()
        {
            portfolio.Add(new PortfolioItem() { Index = "OTP", Volume = 10 });
            portfolio.Add(new PortfolioItem() { Index = "ZWACK", Volume = 10 });
            portfolio.Add(new PortfolioItem() { Index = "ELMU", Volume = 10 });
            dataGridView2.DataSource = portfolio;

        }
        private decimal GetPortfolioValue(DateTime date)
        {
            decimal value = 0;
            foreach (var item in portfolio)
            {
                var last = (from x in ticks
                            where item.Index == x.Index.Trim()
                               && date <= x.TradingDay
                            select x)
                            .First();
                value += (decimal)last.Price * item.Volume;
            }
            return value;
        }
    }
}
