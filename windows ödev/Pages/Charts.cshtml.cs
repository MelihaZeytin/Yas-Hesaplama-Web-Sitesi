using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using windows_ödev.Data;
using windows_ödev.Models.Domain;
namespace windows_ödev.Pages
{
    public class ChartsModel : PageModel
    {

        private readonly RazorPagesDemoDbContext dbContext;

        public ChartsModel(RazorPagesDemoDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public string GenderDistributionJson { get; set; }
        public string AgeAveragesJson { get; set; }

        public void OnGet()
        {
            var genderDistribution = GetGenderDistribution();
            GenderDistributionJson = JsonSerializer.Serialize(genderDistribution);

            var ageAverages = GetAgeAverages();
            AgeAveragesJson = JsonSerializer.Serialize(ageAverages);
        }

        private Dictionary<string, double> GetGenderDistribution()
        {
            var genderDistribution = new Dictionary<string, double>();
            int totalCount = dbContext.Persons.Count();
            int maleCount = dbContext.Persons.Count(p => p.Gender == "Erkek");
            int femaleCount = dbContext.Persons.Count(p => p.Gender == "Kadin");

            genderDistribution["Erkek"] = Math.Round((double)maleCount / totalCount * 100,2);
            genderDistribution["Kadin"] = Math.Round((double)femaleCount / totalCount * 100,2);


            return genderDistribution;
        }

        private Dictionary<string, double> GetAgeAverages()
        {
            var ageAverages = new Dictionary<string, double>();

            var ageGroups = new List<Tuple<int, int>>()
            {
                new Tuple<int, int>(0, 15),
                new Tuple<int, int>(15, 30),
                new Tuple<int, int>(30, 45),
                new Tuple<int, int>(45, int.MaxValue)
            };

            foreach (var group in ageGroups)
            {
                var ageRange = group.Item2 == int.MaxValue ? "45+" : $"{group.Item1}-{group.Item2}";

                var personsInGroup = dbContext.Persons
                    .Where(p => p.Age >= group.Item1 && p.Age < group.Item2);

                var ageAverage = personsInGroup.Any()
                    ? personsInGroup.Average(p => p.Age)
                    : 0;

                ageAverages[ageRange] = Math.Round(ageAverage,2);
            }

            return ageAverages;
        }

    }

}
