using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DealershipDotNetAPI.Domain.DTOs
{
    public class VehicleDTO
    {
        public string Name { get; set; } = default!;

        public string Brand { get; set; } = default!;

        public int Year { get; set; } = default!;
    }
}
