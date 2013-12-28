using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECMS.Core.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace ECMS.Core.Utilities.Tests
{
    [TestClass()]
    public class ECMSUtilityTests
    {
        [TestMethod()]
        public void GetSiteIdTest()
        {
            int actual = Utility.GetSiteId("/");
            int expected = 1;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void GetSiteIdNegativeTest()
        {
            int actual = Utility.GetSiteId("www.google.com");
            int expected = -1;
            Assert.AreEqual(expected, actual);
        }
    }
}
