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
    }
}
