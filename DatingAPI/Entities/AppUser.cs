using DatingAPI.Extensions;

namespace DatingAPI.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }//string is optional, we turnaed off the Nullable flag in csproj file.
        //adding Authentication Hash-Salt
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;//setting the default value
        public DateTime LastActive { get; set; }  = DateTime.UtcNow;
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string Interests { get; set; }
        public string LookingFor { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public List<Photo> Photos {get; set;} = new();

        // public int GetAge(){
        //     return DateOfBirth.CalculateAge();
        // }

    }
}