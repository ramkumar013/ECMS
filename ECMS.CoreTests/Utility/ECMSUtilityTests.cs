using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECMS.Core.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace ECMS.Core.Utility.Tests
{
    [TestClass()]
    public class ECMSUtilityTests
    {
        [TestMethod()]
        public void GetSiteIdTest()
        {
            int actual = ECMSUtility.GetSiteId("/");
            int expected = 1;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void GetSiteIdNegativeTest()
        {
            int actual = ECMSUtility.GetSiteId("www.google.com");
            int expected = -1;
            Assert.AreEqual(expected, actual);
        }
    }
}
