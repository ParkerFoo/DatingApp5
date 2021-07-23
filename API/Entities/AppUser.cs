namespace API.Entities
{
    public class AppUser
    {
        public int Id { get; set; } //EF will be able to indetify this is the PK, just make sure call it as 'Id'. Will auto increment
        public string UserName { get; set; }

        public byte[] PasswordHash { get; set; }
        public byte[]  PasswordSalt { get; set; }
        
    }
}