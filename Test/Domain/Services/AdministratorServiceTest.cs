using DealershipDotNetAPI.Domain.DTOs;
using DealershipDotNetAPI.Domain.Entities;
using DealershipDotNetAPI.Domain.Services;
using DealershipDotNetAPI.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Domain.Services
{
    [TestClass]
    public class AdministratorServiceTest
    {
        private ContextDb CreateTestContext()
        {

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            var configuration = builder.Build();

          
            return new ContextDb(configuration);
        }


        [TestMethod]
        public void AddAdministratorTest()
        {
            //Arrange

            var context = CreateTestContext();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE administrators");
            var adm = new Administrator();
            adm.Id = 1;
            adm.Email = "test@test.com";
            adm.Password = "test";
            adm.Profile = "Adm";
           
            var administratorService = new AdministratorService(context);

            //Act
            administratorService.AddAdministrator(adm);

            //Assert
            Assert.AreEqual(1, administratorService.GetAllAdministrators().Count());
            
        }

        [TestMethod]
        public void LoginAdministratorTest_Success()
        {
            // Arrange
            var context = CreateTestContext();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE administrators");
            var adm = new Administrator { Id = 1, Email = "test@test.com", Password = "test", Profile = "Adm" };
            var administratorService = new AdministratorService(context);
            administratorService.AddAdministrator(adm);
            var loginDTO = new LoginDTO { Email = "test@test.com", Password = "test" };

            // Act
            var result = administratorService.Login(loginDTO);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(adm.Id, result?.Id);
        }

        [TestMethod]
        public void LoginAdministratorTest_Failure()
        {
            // Arrange
            var context = CreateTestContext();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE administrators");
            var administratorService = new AdministratorService(context);
            var loginDTO = new LoginDTO { Email = "nonexistent@test.com", Password = "wrongpassword" };

            // Act
            var result = administratorService.Login(loginDTO);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetAdministratorByIdTest()
        {
            // Arrange
            var context = CreateTestContext();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE administrators");
            var adm = new Administrator { Id = 1, Email = "test@test.com", Password = "test", Profile = "Adm" };
            var administratorService = new AdministratorService(context);
            administratorService.AddAdministrator(adm);

            // Act
            var result = administratorService.GetAdministratorById(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(adm.Id, result?.Id);
        }

        [TestMethod]
        public void UpdateAdministratorTest()
        {
            // Arrange
            var context = CreateTestContext();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE administrators");
            var adm = new Administrator { Id = 1, Email = "test@test.com", Password = "test", Profile = "Adm" };
            var administratorService = new AdministratorService(context);
            administratorService.AddAdministrator(adm);
            adm.Email = "updated@test.com";

            // Act
            var result = administratorService.UpdateAdministrator(adm);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("updated@test.com", result?.Email);
        }

        [TestMethod]
        public void DeleteAdministratorTest()
        {
            // Arrange
            var context = CreateTestContext();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE administrators");
            var adm = new Administrator { Id = 1, Email = "test@test.com", Password = "test", Profile = "Adm" };
            var administratorService = new AdministratorService(context);
            administratorService.AddAdministrator(adm);

            // Act
            administratorService.DeleteAdministrator(adm);

            // Assert
            Assert.AreEqual(0, administratorService.GetAllAdministrators().Count());
        }

        [TestMethod]
        public void GetAllAdministratorsTest()
        {
            // Arrange
            var context = CreateTestContext();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE administrators");
            var adm1 = new Administrator { Id = 1, Email = "test1@test.com", Password = "test", Profile = "Adm" };
            var adm2 = new Administrator { Id = 2, Email = "test2@test.com", Password = "test", Profile = "Adm" };
            var administratorService = new AdministratorService(context);
            administratorService.AddAdministrator(adm1);
            administratorService.AddAdministrator(adm2);

            // Act
            var result = administratorService.GetAllAdministrators();

            // Assert
            Assert.AreEqual(2, result.Count());
        }

    }
}
