using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DealershipDotNetAPI.Domain.DTOs
{
    public class AdministratorDTO
    {
        public string Email { get; set; } = default!;

        public string Password { get; set; } = default!;

        public string Profile { get; set; } = default!;
    }
}
