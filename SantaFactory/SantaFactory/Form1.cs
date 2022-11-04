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
        List<Toy> _balls = new List<Toy>();

        private BallFactory _factory;
        public BallFactory Factory
        {
            get { return _factory; }
            set { _factory = value; }
        }
        public Form1()
        {
            InitializeComponent();
            Factory = new BallFactory();
        }

        private void conveyorTimer_Tick(object sender, EventArgs e)
        {
            int maxLeft = 0;
            foreach(Ball b in _balls)
            {
                b.MoveToy();
                if(b.Left > maxLeft)
                {
                    maxLeft = b.Left;
                }
            }
            if(maxLeft > 1000)
            {
                Toy firstBall = _balls[0];
                _balls.Remove(firstBall);
                mainPanel.Controls.Remove(firstBall);
            }
        }
        private void createTimer_Tick(object sender, EventArgs e)
        {
            var b = Factory.CreateNew();
            _balls.Add(b);
            mainPanel.Controls.Add(b);
            b.Left = -b.Width;
        }
    }
}
