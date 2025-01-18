using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DealershipDotNetAPI.Domain.Entities

/// Represents the Administrator entity in the system.
/// This class is used to manage administrator-related data, including their email, password, and profile.
/// When using Entity Framework Core (EF Core), this class will be mapped to a database table named "Administrators" by default.
{
    public class Administrator
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } = default!;

        [Required]
        [StringLength(255)]
        public string Email { get; set; } = default!;

        [Required]
        [StringLength(50)]
        public string Password { get; set; } = default!;

        [StringLength(10)]
        public string Profile { get; set; } = default!;
    }
}
