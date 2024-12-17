using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DealershipDotNetAPI.Domain.Entities
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
