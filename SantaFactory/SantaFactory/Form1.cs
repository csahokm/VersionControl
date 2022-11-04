using SantaFactory.Abstractions;
using SantaFactory.Entities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SantaFactory
{
    public partial class Form1 : Form
    {
        List<Toy> _toys = new List<Toy>();
        private Toy _nextToy;

        private IToyFactory _factory;
        public IToyFactory Factory
        {
            get { return _factory; }
            set
            {
                _factory = value;
                DisplayNext();
            }
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
            t.Top = 150;
            _toys.Add(t);
            mainPanel.Controls.Add(t);
            t.Left = -t.Width;
        }

        private void btnCar_Click(object sender, EventArgs e)
        {
            Factory = new CarFactory();
        }

        private void btnBall_Click(object sender, EventArgs e)
        {
            Factory = new BallFactory();
        }
        private void DisplayNext()
        {
            if(_nextToy != null)
            {
                mainPanel.Controls.Remove(_nextToy);
            }
            _nextToy = Factory.CreateNew();
            _nextToy.Top = label1.Top + label1.Height + 20;
            _nextToy.Left = label1.Left + 15;
            mainPanel.Controls.Add(_nextToy);
        }

        private void btnBallColor_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var colorPicker = new ColorDialog();

            colorPicker.Color = btnBallColor.BackColor;
            if (colorPicker.ShowDialog() != DialogResult.OK)
                return;
            button.BackColor = colorPicker.Color;
        }
    }
}
