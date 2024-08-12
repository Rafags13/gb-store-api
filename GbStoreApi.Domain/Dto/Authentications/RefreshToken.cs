namespace GbStoreApi.Domain.Dto.Authentications
{
    public class RefreshToken
    {
        public required string Token { get; set; }
        public DateTime TokenCreated { get; set; } = DateTime.Now;
        public DateTime TokenExpires { get; set; }
    }
}
