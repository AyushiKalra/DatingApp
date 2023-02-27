using System.ComponentModel.DataAnnotations.Schema;

namespace DatingAPI.Entities
{
    [Table("Photos")]
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }        
        public bool IsMain { get; set; }
        public string PublicId { get; set; }

        //to get the full relationship property of EF.
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}