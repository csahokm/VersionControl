using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantaFactory.Entities
{
    class BallFactory
    {
        public Ball CreateNew()
        {
            Ball b = new Ball();
            return b;
        }
    }
}
