using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;


namespace SHH.UI.Curve.Core
{
    public class DrawingLineEventArgs
    {
        public List<BrokenLine> BrokenLines { get; set; }
        public List<BrokenLine> TimeInterval { get; set; }
        public List<BrokenLine> ScaleRatioAxisX { get; set; }
        public List<BrokenLine> ScaleRatioAxisY { get; set; }
    }
}
