using DealershipDotNetAPI.Domain.Entities;
using DealershipDotNetAPI.Domain.Interfaces;
using DealershipDotNetAPI.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace DealershipDotNetAPI.Domain.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly ContextDb _contextDb;

        // Constructor that initializes the database context
        public VehicleService(ContextDb contextDb)
        {
            _contextDb = contextDb;
        }

        /// <summary>
        /// Retrieves a paginated list of vehicles with optional filters by name and brand.
        /// </summary>
        /// <param name="page">Page number for pagination (default is 1).</param>
        /// <param name="name">Optional filter for vehicle name.</param>
        /// <param name="brand">Optional filter for vehicle brand.</param>
        /// <returns>A list of vehicles that match the specified criteria.</returns>
        public List<Vehicle> AllVehicles(int page = 1, string? name = null, string? brand = null)
        {
            var query = _contextDb.Vehicles.AsQueryable();
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(v => EF.Functions.Like(v.Name.ToLower(), $"%{name}%"));
            }
            int itemsPerPage = 10;

            query = query.Skip((page - 1) * itemsPerPage).Take(itemsPerPage);

            return query.ToList();
        }

        /// <summary>
        /// Deletes a specific vehicle from the database.
        /// </summary>
        /// <param name="vehicle">The vehicle entity to be deleted.</param>
        public void Delete(Vehicle vehicle)
        {
            _contextDb.Vehicles.Remove(vehicle);
            _contextDb.SaveChanges();
        }

        /// <summary>
        /// Retrieves a vehicle by its unique identifier.
        /// </summary>
        /// <param name="id">The unique ID of the vehicle.</param>
        /// <returns>The vehicle entity if found; otherwise, null.</returns>
        public Vehicle? GetById(int id)
        {
            return _contextDb.Vehicles.Where(v => v.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// Adds a new vehicle to the database.
        /// </summary>
        /// <param name="vehicle">The vehicle entity to be saved.</param>
        public void Save(Vehicle vehicle)
        {
            _contextDb.Vehicles.Add(vehicle);
            _contextDb.SaveChanges();
        }

        /// <summary>
        /// Updates an existing vehicle's details in the database.
        /// </summary>
        /// <param name="vehicle">The updated vehicle entity.</param>
        public void Update(Vehicle vehicle)
        {
            _contextDb.Vehicles.Update(vehicle);
            _contextDb.SaveChanges();
        }
    }
}
