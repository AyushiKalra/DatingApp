namespace DatingAPI.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }//string is optional, we turnaed off the Nullable flag in csproj file.
        
    }
}