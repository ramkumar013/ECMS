using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECMS.Services.ValidUrlService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECMS.Core.Entities;
using ECMS.Services;
using ECMS.Core;
using ECMS.Core.Framework;
using Newtonsoft.Json;
namespace ECMS.Services.ValidUrlService.Tests
{
    [TestClass()]
    public class ValidUrlFileRepositoryTests
    {
        [TestMethod()]
        public void GetById_InProcCachingService_Test()
        {
            DependencyManager.CachingService = new InProcCachingService();
            ValidUrlFileRepository fileRepository = new ValidUrlFileRepository();
            ValidUrl url = fileRepository.GetByFriendlyUrl(1, "/flights/cheap-flights-to-new-york-city");
            Assert.AreEqual("/flights/destination-city", url.View);
            Assert.AreEqual(true, url.Active);
            Assert.AreEqual(true, url.Index);
            Assert.AreEqual(200, url.StatusCode);
        }
        [TestMethod()]
        public void GetById_InProcCachingService_SlashURL_Test()
        {
            DependencyManager.CachingService = new InProcCachingService();
            ValidUrlFileRepository fileRepository = new ValidUrlFileRepository();
            ValidUrl url = fileRepository.GetByFriendlyUrl(1, "/");
            Assert.AreEqual("/flights/home", url.View);
            Assert.AreEqual(true, url.Active);
            Assert.AreEqual(true, url.Index);
            Assert.AreEqual(200, url.StatusCode);
        }

        [TestMethod()]
        public void VerifyContentWithDefault_Assert_Dynamic_Properties_Test()
        {
            DependencyManager.CachingService = new InProcCachingService();
            ValidUrlFileRepository fileRepository = new ValidUrlFileRepository();
            dynamic contentItem = JsonConvert.DeserializeObject(@"{'Title':'This is page title','Keywords':'keywords... keywords..... keywords.....','Description':'This is a test content.'}");
            dynamic actual = ContentRepositoryBase.VerifyContentWithDefault(contentItem);

            Assert.AreEqual(contentItem.Title, actual.Title);
            Assert.AreEqual(contentItem.Keywords, actual.Keywords);
            Assert.AreEqual(contentItem.Description, actual.Description);
        }

        [TestMethod()]
        public void VerifyContentWithDefault_Assert_With_HardCoded_Values_Test()
        {
            // Note : This is expected to fail after change in method.
            DependencyManager.CachingService = new InProcCachingService();
            ValidUrlFileRepository fileRepository = new ValidUrlFileRepository();
            dynamic contentItem = JsonConvert.DeserializeObject(@"{'Title':'This is page title','Keywords':'keywords... keywords..... keywords.....','Description':'This is a test content.'}");
            dynamic actual = ContentRepositoryBase.VerifyContentWithDefault(contentItem);

            Assert.AreEqual("This is page title.. ok pass", actual.Title.ToString());
            Assert.AreEqual("keywords... keywords..... keywords....... ok pass", actual.Keywords.ToString());
            Assert.AreEqual("This is a test content... ok pass", actual.Description.ToString());
        }
    }
}
