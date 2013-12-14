using ECMS.HttpModules;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Web;

namespace Eclipse.HttpModules.Test
{
    
    
    /// <summary>
    ///This is a test class for UtilityTest and is intended
    ///to contain all UtilityTest Unit Tests
    ///</summary>
    [TestClass()]
    public class UtilityTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for IsValidUrlForRewrite
        ///</summary>
        [TestMethod()]
        public void IsValidUrlForRewrite_SlashUrl_Test()
        {
            var httpContext = new Mock<HttpContextBase>();
            var httpRequest = new Mock<HttpRequestBase>();
            httpRequest.Setup(x => x.Url).Returns(new Uri("http://www.explotravel.com/"));
            httpContext.Setup(x => x.Request).Returns(httpRequest.Object);
            bool expected = true; 
            bool actual=false;
            //actual = Utility.IsValidUrlForRewrite(httpContext.Object);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for IsValidUrlForRewrite
        ///</summary>
        [TestMethod()]
        public void IsValidUrlForRewrite_InvalidUrl_Test()
        {
            var httpContext = new Mock<HttpContextBase>();
            var httpRequest = new Mock<HttpRequestBase>();
            httpRequest.Setup(x => x.Url).Returns(new Uri("http://www.explotravel.com/myservice.asmx"));
            httpContext.Setup(x => x.Request).Returns(httpRequest.Object);
            bool expected = false;
            bool actual = true;
            //actual = Utility.IsValidUrlForRewrite(httpContext.Object);
            Assert.AreEqual(expected, actual);
        }
    }
}
