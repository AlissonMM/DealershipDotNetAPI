using DealershipDotNetAPI.Domain.Entities;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Domain.Entities
{
    [TestClass]
    internal class AdministratorTest
    {
        [TestMethod]
        public void TestGetSetProperties()
        {
            //Arrange
            var adm = new Administrator();

            //Act
            adm.Id = 10;
            adm.Email = "test@test.com";
            adm.Password = "test";
            adm.Profile = "Adm";

            //Assert
            Assert.AreEqual(10, adm.Id);
            Assert.AreEqual("test@test.com", adm.Email);
            Assert.AreEqual("test", adm.Password);
            Assert.AreEqual("Adm", adm.Profile);
        }
    }
}
