using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Types
{
    public class Thickness
    {
        public Thickness(double top, double left, double right, double bottom)
        {
            Top = top;
            Left = left;
            Right = right;
            Bottom = bottom;
        }

        public Thickness()
        {
        }

        public double Top { get; set; }

        public double Left { get; set; }
        
        public double Right { get; set; }

        public double Bottom { get; set; }
    }
}
