using DealershipDotNetAPI.Domain.DTOs;
using DealershipDotNetAPI.Domain.Entities;

namespace DealershipDotNetAPI.Domain.Interfaces
{

    // The `IAdministratorService` interface defines the contract for services 
    // related to the `Administrator` entity. It ensures that any class implementing 
    // this interface provides a `Login` method that takes a `LoginDTO` object 
    public interface IAdministratorService
    {
        Administrator? Login(LoginDTO loginDTO);
    }
}
