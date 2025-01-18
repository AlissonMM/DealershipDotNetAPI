using DealershipDotNetAPI.Domain.Entities;

namespace DealershipDotNetAPI.Domain.Interfaces
{
    public interface IVehicleService
    {
        List<Vehicle>AllVehicles(int? page = 1, string? name = null, string? brand = null);

        Vehicle? GetById(int id);

        void Save(Vehicle vehicle);

        void Update(Vehicle vehicle);

        void Delete (Vehicle vehicle);
    }
}
