using System.ComponentModel.DataAnnotations.Schema;
using static System.Net.Mime.MediaTypeNames;

namespace windows_ödev.Models.ViewModels
{
    public class PersonViewModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string? PhotoData { get; set; }

        [NotMapped]
        public IFormFile PhotoFile { get; set; }
    }
}
