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
    }
}
