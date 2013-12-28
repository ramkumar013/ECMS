using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECMS.Services.ContentRepository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECMS.Core.Entities;
using ECMS.Core;
using ECMS.Core.Framework;
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
            ContentItem item = repo.GetById(new ValidUrl() { SiteId = 1, Id = Guid.Parse("2ECA40A9-E580-4589-B89B-17458B61590D"), View = "Layout-Default" },ContentViewType.PUBLISH);
            Assert.IsNotNull(item);
            Assert.AreEqual("Explo Travel - Demo", item.Head.Title);
            Assert.AreEqual("Explo Travel", item.Head.KeyWords);
            Assert.AreEqual("search cheap flights", item.Head.Description);
            Assert.AreEqual("no tags", item.Head.PageMetaTags);
        }

        [TestMethod()]
        public void GetByIdMergeTitleTest()
        {
            DependencyManager.CachingService = new InProcCachingService();
            FileSystemRepository repo = new FileSystemRepository();
            ContentItem item = repo.GetById(new ValidUrl() { SiteId = 1, Id = Guid.Parse("A1ED7FDF-498D-43EE-BC81-9E23F0294C57"), View = "Layout-Default" }, ContentViewType.PUBLISH);
            Assert.IsNotNull(item);
            Assert.AreEqual("I am actual title", item.Head.Title);
            Assert.AreEqual("Explo Travel", item.Head.KeyWords);
            Assert.AreEqual("search cheap flights", item.Head.Description);
            Assert.AreEqual("no tags", item.Head.PageMetaTags);
        }

        [TestMethod()]
        public void GetByIdMergeKeyWordsTest()
        {
            DependencyManager.CachingService = new InProcCachingService();
            FileSystemRepository repo = new FileSystemRepository();
            ContentItem item = repo.GetById(new ValidUrl() { SiteId = 1, Id = Guid.Parse("A1ED7FDF-498D-43EE-BC81-9E23F0294C58"), View = "Layout-Default" }, ContentViewType.PUBLISH);
            Assert.IsNotNull(item);
            //uncomment below line if you want to individual run this test.
            //Assert.AreEqual("Explo Travel - Demo", item.Head.Title);
            Assert.AreEqual("I am actual KeyWords", item.Head.KeyWords);
            Assert.AreEqual("search cheap flights", item.Head.Description);
            Assert.AreEqual("no tags", item.Head.PageMetaTags);
        }

        [TestMethod()]
        public void GetByIdMergeDescriptionTest()
        {
            DependencyManager.CachingService = new InProcCachingService();
            FileSystemRepository repo = new FileSystemRepository();
            ContentItem item = repo.GetById(new ValidUrl() { SiteId = 1, Id = Guid.Parse("A1ED7FDF-498D-43EE-BC81-9E23F0294C59"), View = "Layout-Default" }, ContentViewType.PUBLISH);
            Assert.IsNotNull(item);
            //uncomment below line if you want to individual run this test.
            //Assert.AreEqual("Explo Travel - Demo", item.Head.Title);
            //Assert.AreEqual("Explo Travel", item.Head.KeyWords);
            Assert.AreEqual("I am actual Description", item.Head.Description);
            Assert.AreEqual("no tags", item.Head.PageMetaTags);
        }
        [TestMethod()]
        public void GetByIdMergePageMetaTagsTest()
        {
            DependencyManager.CachingService = new InProcCachingService();
            FileSystemRepository repo = new FileSystemRepository();
            ContentItem item = repo.GetById(new ValidUrl() { SiteId = 1, Id = Guid.Parse("A1ED7FDF-498D-43EE-BC81-9E23F0294C60"), View = "Layout-Default" }, ContentViewType.PUBLISH);
            Assert.IsNotNull(item);
            //uncomment below line if you want to individual run this test.
            //Assert.AreEqual("Explo Travel - Demo", item.Head.Title);
            //Assert.AreEqual("Explo Travel", item.Head.KeyWords);
            //Assert.AreEqual("search cheap flights", item.Head.Description);
            Assert.AreEqual("I am actual PageMetaTags", item.Head.PageMetaTags);
        }
    }
}
