namespace DealershipDotNetAPI.Domain.ModelViews
{
    public class LoggedAdmin
    {
        public string Email { get; set; } = default!;

        public string Profile { get; set; } = default!;

        public string JwtToken { get; set; } = default!;
    }
}
