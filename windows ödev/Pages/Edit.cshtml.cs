using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using windows_ödev.Data;
using windows_ödev.Models.ViewModels;

namespace windows_ödev.Pages
{
    public class EditModel : PageModel
    {

        private readonly RazorPagesDemoDbContext dbContext;

        [BindProperty]
        public EditPersonViewModel EditPersonViewModel { get; set; }
        public EditModel(RazorPagesDemoDbContext dbContext) 
        {
        
            this.dbContext = dbContext;
        }  
        public void OnGet(Guid id)
        {
            var person = dbContext.Persons.Find(id);

            if(person != null)
            {
                // Convert Domain Model to View Model
                EditPersonViewModel = new EditPersonViewModel()
                {
                    Id = person.Id,
                    Name = person.Name,
                    Surname = person.Surname,
                    BirthDate = person.BirthDate,
                    Gender = person.Gender,
                    Age = person.Age,
                    PhotoData = person.PhotoData,
                    PhotoFile = person.PhotoFile,
                };
            }
        }

        public void OnPostUpdate()
        {
            if(EditPersonViewModel != null)
            {
                var existingPerson = dbContext.Persons.Find(EditPersonViewModel.Id);


            
                if(existingPerson != null)
                {
                    var today = DateTime.Now;

                    int age = today.Year - EditPersonViewModel.BirthDate.Year;
                    int monthDiff = today.Month - EditPersonViewModel.BirthDate.Month;
                    if (monthDiff < 0 || monthDiff == 0 && today.Day < EditPersonViewModel.BirthDate.Day)
                    {
                        age--;
                    }
                    EditPersonViewModel.Age = age;

                    // Convert ViewModel to DomainModel

                    existingPerson.Id = EditPersonViewModel.Id;
                    existingPerson.Name = EditPersonViewModel.Name;
                    existingPerson.Surname = EditPersonViewModel.Surname;
                    existingPerson.BirthDate = EditPersonViewModel.BirthDate;
                    existingPerson.Gender = EditPersonViewModel.Gender;
                    existingPerson.Age = EditPersonViewModel.Age;
                    existingPerson.PhotoFile = EditPersonViewModel.PhotoFile;


                    
                    byte[] bytes = null;

                    if (existingPerson.PhotoFile != null)
                    {
                        using (Stream fs = existingPerson.PhotoFile.OpenReadStream())
                        {
                            using (BinaryReader br = new BinaryReader(fs))
                            {
                                bytes = br.ReadBytes((Int32)fs.Length);
                            }
                            existingPerson.PhotoData = Convert.ToBase64String(bytes, 0, bytes.Length);
                        }
                    }


                    if (string.IsNullOrEmpty(existingPerson.Name) || string.IsNullOrEmpty(existingPerson.Surname) || string.IsNullOrEmpty(existingPerson.Gender) || DateTime.MinValue == existingPerson.BirthDate)
                    {
                        ViewData["AlertMessage"] = "Tum bilgilerinizi giriniz.";
                        return;
                    }


                    if (existingPerson.BirthDate > DateTime.Now)
                    {
                        ViewData["AlertMessage"] = "Dogum tarihiniz, bugunun tarihinden ileri bir tarih olamaz.";
                        return;
                    }

                    if (today.Year - existingPerson.BirthDate.Year >= 120)
                    {
                        ViewData["AlertMessage"] = "Dogum tarihiniz cok eski bir tarih olamaz.";
                        return;
                    }

                    dbContext.SaveChanges();

                    ViewData["Message"] = "Başarıyla Güncellendi.";
                }
            
            
            }


        }

        public IActionResult OnPostDelete()
        {
            var existingPerson = dbContext.Persons.Find(EditPersonViewModel.Id);

            if(existingPerson != null)
            {
                dbContext.Persons.Remove(existingPerson);
                dbContext.SaveChanges();

                return RedirectToPage("/Tablo");
            }

            return Page();
        }
    }
}
