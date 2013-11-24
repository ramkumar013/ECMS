using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECMS.Services.ContentRepository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECMS.Core.Entities;
using ECMS.Core;
namespace ECMS.Services.ContentRepository.Tests
{
    [TestClass()]
    public class FileSystemRepositoryTests
    {
        [TestMethod()]
        public void GetByIdTest()
        {
            DependencyManager.CachingService = new InProcCachingService();
            FileSystemRepository repo = new FileSystemRepository();
            ContentItem item = repo.GetById(new ValidUrl() { Id = Guid.Parse("2ECA40A9-E580-4589-B89B-17458B61590D"), View = "Layout-Default" });
            Assert.IsNotNull(item);
        }
    }
}
