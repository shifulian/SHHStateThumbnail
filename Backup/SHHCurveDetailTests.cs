using Microsoft.VisualStudio.TestTools.UnitTesting;
using SHH.UI.Curve.Detail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHH.UI.Curve.Detail.Tests
{
    [TestClass()]
    public class SHHCurveDetailTests
    {
        [TestMethod()]
        public void RePaintTest()
        {
            new SHHCurveDetail().RePaint();
        }
    }
}