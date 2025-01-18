using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DealershipDotNetAPI.Domain.Entities

/// Represents the Administrator entity in the system.
/// This class is used to manage administrator-related data, including their email, password, and profile.
/// When using Entity Framework Core (EF Core), this class will be mapped to a database table named "Administrators" by default.
{
    public class Vehicle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } = default!;

        [Required]
        [StringLength(150)]
        public string Name { get; set; } = default!;

        [Required]
        [StringLength(100)]
        public string Brand { get; set; } = default!;

        [Required]
        public int Year { get; set; } = default!;
    }
}
