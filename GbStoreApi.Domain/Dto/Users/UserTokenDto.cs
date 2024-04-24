namespace GbStoreApi.Domain.Dto.Users
{
    public class UserTokenDto
    {
        public UserTokenDto(int id, string name, string email, int typeOfUser)
        {
            Id = id;
            Name = name;
            Email = email;
            TypeOfUser = typeOfUser;
        }
        public UserTokenDto() { }
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int TypeOfUser { get; set; }
    }
}
