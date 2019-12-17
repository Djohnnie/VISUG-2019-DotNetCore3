using System.ComponentModel.DataAnnotations;

namespace _04_TieredCompilation.Models
{
    public class Artist
    {
        public int ArtistId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}