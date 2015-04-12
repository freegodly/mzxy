using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 梦之西游.Controls
{
    interface GameObject
    {
        double CenterX { get; set; }
        double CenterY { get; set; }
        double X { get; set; }
        double Y { get; set; }
        double Left { get; set; }
        double Top { get; set; }
        int ZIndex { get; set; }
        double Width_ { get; set; }
        double Height_ { get; set; }
    }
}
