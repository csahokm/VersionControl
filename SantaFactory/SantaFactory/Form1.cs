using SantaFactory.Abstractions;
using SantaFactory.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SantaFactory
{
    public partial class Form1 : Form
    {
        List<Toy> _toys = new List<Toy>();

        private IToyFactory _factory;
        public IToyFactory Factory
        {
            get { return _factory; }
            set { _factory = value; }
        }
        public Form1()
        {
            InitializeComponent();
            Factory = new CarFactory();
        }

        private void conveyorTimer_Tick(object sender, EventArgs e)
        {
            int maxLeft = 0;
            foreach(Toy t in _toys)
            {
                t.MoveToy();
                if(t.Left > maxLeft)
                {
                    maxLeft = t.Left;
                }
            }
            if(maxLeft > 1000)
            {
                Toy firstToy = _toys[0];
                _toys.Remove(firstToy);
                mainPanel.Controls.Remove(firstToy);
            }
        }
        private void createTimer_Tick(object sender, EventArgs e)
        {
            var t = Factory.CreateNew();
            _toys.Add(t);
            mainPanel.Controls.Add(t);
            t.Left = -t.Width;
        }
    }
}
