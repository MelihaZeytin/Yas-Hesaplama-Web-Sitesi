using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Principal;
using System.Text.Json;
using windows_ödev.Data;
using windows_ödev.Models.Domain;
using Microsoft.Extensions.Configuration;

namespace windows_ödev.Pages
{
    public class TabloModel : PageModel
    {


        private readonly RazorPagesDemoDbContext dbContext;
        private readonly IConfiguration Configuration;

        public TabloModel(RazorPagesDemoDbContext dbContext, IConfiguration configuration)
        {
            this.dbContext = dbContext;
            Configuration = configuration;
        }

       //public List<Models.Domain.Person> Persons { get; set; }
        public PaginatedList<Person> Persons { get; set; }

        public string NameSort { get; set; }
        public string SurnameSort { get; set; }
        public string DateSort { get; set; }
        public string AgeSort { get; set; }
        public string GenderSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }




        //public void OnGet()
        //{
        //    Persons = dbContext.Persons.ToList();
        //}

        public async Task OnGetAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex)
        {
            CurrentSort = sortOrder;

            // using System;
            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            DateSort = sortOrder == "Date" ? "date_desc" : "Date";
            AgeSort = sortOrder == "Age" ? "age_desc" : "Age";
            GenderSort = sortOrder == "Gender" ? "gender_desc" : "Gender";
            SurnameSort = sortOrder == "Surname" ? "surname_desc" : "Surname";

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            CurrentFilter = searchString;

            IQueryable<Person> person = from s in dbContext.Persons
                                            select s;


            if (!String.IsNullOrEmpty(searchString))
            {
                person = person.Where(s => s.Name.Contains(searchString)
                                       || s.Surname.Contains(searchString)
                                       || s.Age.ToString().Contains(searchString)
                                       || s.Gender.Contains(searchString)
                                       || s.BirthDate.Date.ToString().Contains(searchString)
                                       );
            }


            switch (sortOrder)
            {

                case "name_desc":
                    person = person.OrderByDescending(s => s.Name);
                    break;
                case "Date":
                    person = person.OrderBy(s => s.BirthDate);
                    break;
                case "date_desc":
                    person = person.OrderByDescending(s => s.BirthDate);
                    break;
                case "Age":
                    person = person.OrderBy(s => s.Age);
                    break;
                case "age_desc":
                    person = person.OrderByDescending(s => s.Age);
                    break;
                case "Gender":
                    person = person.OrderBy(s => s.Gender);
                    break;
                case "gender_desc":
                    person = person.OrderByDescending(s => s.Gender);
                    break;
                case "Surname":
                    person = person.OrderBy(s => s.Surname);
                    break;
                case "surname_desc":
                    person = person.OrderByDescending(s => s.Surname);
                    break;
                default:
                    person = person.OrderBy(s => s.Name);
                    break;
            }

            //Persons = await person.AsNoTracking().ToListAsync();
            var pageSize = Configuration.GetValue("PageSize", 4);
            Persons = await PaginatedList<Person>.CreateAsync(
                person.AsNoTracking(), pageIndex ?? 1, pageSize);


        }



    }
}
