using SantaFactory.Abstractions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantaFactory.Entities
{
    public class Present : Toy
    {
        protected override void DrawImage(Graphics g)
        {
            g.FillRectangle(PresentColor, 0, 0, Width, Height);
            g.FillRectangle(RibbonColor, 0, Height/2-5, Width, (Height/2-5)/2);
            g.FillRectangle(RibbonColor, Width/2-5, 0, (Width/2-5)/2, Height);
        }
        public SolidBrush PresentColor { get; private set; }
        public SolidBrush RibbonColor { get; private set; }
        public Present(Color ribbon, Color box)
        {
            PresentColor = new SolidBrush(box);
            RibbonColor = new SolidBrush(ribbon);
        }
    }
}
