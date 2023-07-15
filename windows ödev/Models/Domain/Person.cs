using System.ComponentModel.DataAnnotations.Schema;

namespace windows_ödev.Models.Domain
{
    public class Person
    {
        public Guid Id { get; set; }
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
