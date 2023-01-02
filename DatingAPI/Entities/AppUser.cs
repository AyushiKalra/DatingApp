namespace DatingAPI.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }//string is optional, we turnaed off the Nullable flag in csproj file.

        //adding Authentication Hash-Salt
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        
    }
}