using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECMS.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECMS.Core.Entities;
namespace ECMS.Services.Tests
{
    [TestClass()]
    public class ValidUrlMongoDBRepositoryTests
    {
        ValidUrlMongoDBRepository _mongodbRepo = new ValidUrlMongoDBRepository();

        [TestMethod()]
        public void GetByFriendlyUrlTest()
        {
            ValidUrl expected = new ValidUrl
            {
                Id = Guid.NewGuid(),
                SiteId = 0,
                Action = "/Template/Compose",
                Active = true,
                FriendlyUrl = "/flights/",
                Index = true,
                StatusCode = 200,
                View = "/home/flights.cshtml",
                LastModified=DateTime.Now
            };
            _mongodbRepo.Save(expected);

            ValidUrl actual = _mongodbRepo.GetByFriendlyUrl(expected.SiteId, expected.FriendlyUrl);
            Assert.AreEqual(actual.Id , expected.Id);
        }

        [TestMethod()]
        public void SaveTest()
        {
            ValidUrl expected = new ValidUrl
            {
                Id = Guid.NewGuid(),
                SiteId = 0,
                Action = "/Template/Compose",
                Active = true,
                FriendlyUrl = "/flights/",
                Index = true,
                StatusCode = 200,
                View = "/home/flights.cshtml",
                LastModified = DateTime.Now
            };
            _mongodbRepo.Save(expected);
        }

        [TestMethod()]
        public void UpdateTest()
        {
            // arrange
            ValidUrl temp1 = new ValidUrl
            {
                Id = Guid.NewGuid(),
                SiteId = 0,
                Action = "/Template/Compose",
                Active = true,
                FriendlyUrl = "/flights/",
                Index = true,
                StatusCode = 200,
                View = "/home/flights.cshtml",
                LastModified = DateTime.Now
            };
            _mongodbRepo.Save(temp1);

            ValidUrl expected = _mongodbRepo.GetByFriendlyUrl(temp1.SiteId, temp1.FriendlyUrl);
            expected.FriendlyUrl = "/flights/v1";
            expected.StatusCode = 200;
            expected.Active = false;

            // act
            _mongodbRepo.Update(expected);
            ValidUrl actual = _mongodbRepo.GetByFriendlyUrl(expected.SiteId, expected.FriendlyUrl);

            // assert
            Assert.AreEqual(actual.FriendlyUrl, expected.FriendlyUrl);
            Assert.AreEqual(actual.StatusCode, expected.StatusCode);
            Assert.AreEqual(actual.Active, expected.Active);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            // arrange
            ValidUrl temp1 = new ValidUrl
            {
                Id = Guid.NewGuid(),
                SiteId = 0,
                Action = "/Template/Compose",
                Active = true,
                FriendlyUrl = "/flights/v5",
                Index = true,
                StatusCode = 200,
                View = "/home/flights.cshtml",
                LastModified = DateTime.Now
            };
            _mongodbRepo.Save(temp1);

            // act
            _mongodbRepo.Delete(temp1);

            // assert
            var actual = _mongodbRepo.GetByFriendlyUrl(temp1.SiteId, temp1.FriendlyUrl);

            Assert.IsNull(actual);
        }

        [TestMethod()]
        public void GetAllTest()
        {
            List<ValidUrl> urls = _mongodbRepo.GetAll(0, false);
        }
    }
}
