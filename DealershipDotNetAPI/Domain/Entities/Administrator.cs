namespace DealershipDotNetAPI.Domain.Entities
{
    public class Administrator
    {
        public int Id { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string Profile { get; set; } = default!;
    }
}
