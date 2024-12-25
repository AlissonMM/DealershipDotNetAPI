using DealershipDotNetAPI.Domain.DTOs;
using DealershipDotNetAPI.Domain.Entities;
using DealershipDotNetAPI.Domain.Interfaces;
using DealershipDotNetAPI.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace DealershipDotNetAPI.Domain.Services
{
    // The `AdministratorService` class provides business logic and functionality 
    // related to the `Administrator` entity. It implements the `IAdministratorService` interface
    // to ensure a clear contract for services dealing with administrators. 
    public class AdministratorService : IAdministratorService
    {
        // Dependency on the application's DbContext to perform database operations.
        private readonly ContextDb _contextDb;

        // Constructor: Injects the application's database context via dependency injection.
        public AdministratorService(ContextDb contextDb) {
            _contextDb = contextDb;
        }

        // Login Method:
        // Takes a `LoginDTO` as input, queries the database for an administrator 
        // matching the provided email and password, and returns the matching `Administrator` object.
        // If no match is found, it returns null, (thats what the "?" do).
        public Administrator? Login(LoginDTO loginDTO)
        {
            var adm = _contextDb.Administrators.Where(a => a.Email == loginDTO.Email && a.Password == loginDTO.Password).FirstOrDefault();
            return adm;
            
                
            
        }
    }
}
