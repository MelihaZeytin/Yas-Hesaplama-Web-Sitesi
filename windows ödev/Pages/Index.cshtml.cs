using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using windows_ödev.Data;
using windows_ödev.Models.Domain;
using windows_ödev.Models.ViewModels;

namespace windows_ödev.Pages
{
    public class IndexModel : PageModel
    {

        private readonly RazorPagesDemoDbContext dbContext;

        public IndexModel(RazorPagesDemoDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        [BindProperty]
        public PersonViewModel AddPersonRequest { get; set; }




        public void OnGet()
        {
           
        }


        public void OnPost()
        {
            // Convert ViewModel to DomainModel
            var personDomainModel = new Person
            {
                Name = AddPersonRequest.Name,
                Surname = AddPersonRequest.Surname,
                BirthDate = AddPersonRequest.BirthDate,
                Gender = AddPersonRequest.Gender,
                PhotoFile = AddPersonRequest.PhotoFile
            };

            byte[] bytes = null;

            if(AddPersonRequest.PhotoFile != null)
            {
                using (Stream fs = personDomainModel.PhotoFile.OpenReadStream())
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        bytes = br.ReadBytes((Int32)fs.Length);
                    }
                    personDomainModel.PhotoData = Convert.ToBase64String(bytes, 0, bytes.Length);
                }
            }



            var today = DateTime.Now;


            if (string.IsNullOrEmpty(personDomainModel.Name) || string.IsNullOrEmpty(personDomainModel.Surname) || string.IsNullOrEmpty(personDomainModel.Gender) || DateTime.MinValue == personDomainModel.BirthDate)
            {
                ViewData["AlertMessage"] = "Tum bilgilerinizi giriniz.";
                return;
            }


            if (personDomainModel.BirthDate > DateTime.Now)
            {
                ViewData["AlertMessage"] = "Dogum tarihiniz, bugunun tarihinden ileri bir tarih olamaz.";
                return;
            }

            if (today.Year - personDomainModel.BirthDate.Year >= 120)
            {
                ViewData["AlertMessage"] = "Dogum tarihiniz cok eski bir tarih olamaz.";
                return;
            }


            int age = today.Year - personDomainModel.BirthDate.Year;
            int monthDiff = today.Month - personDomainModel.BirthDate.Month;
            if(monthDiff < 0 || monthDiff == 0 && today.Day < personDomainModel.BirthDate.Day)
            {
                age--;
            }

            personDomainModel.Age = age;
            dbContext.Persons.Add(personDomainModel);
            dbContext.SaveChanges();

            ViewData["Message"] = "Başarıyla eklendi.";



        }


}
}