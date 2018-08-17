using Microsoft.VisualStudio.TestTools.UnitTesting;
using SHH.UI.Curve.Thumbnail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHH.UI.Curve.Thumbnail.Tests
{
    [TestClass()]
    public class SHHCurveThumbnailTests
    {
        [TestMethod()]
        public void DrawingLinesOnCanvasTest()
        {
            try
            {
                new SHHCurveThumbnail().DrawingLinesOnCanvas();
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod()]
        public void InitTest()
        {
            try
            {
                new SHHCurveThumbnail().Init();
            }
            catch (Exception e)
            {
                if (e.Message != "Load Template Failed")
                    Assert.Fail(e.Message);
            }
        }
    }
}