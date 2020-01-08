using MovieApiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApiProject.Services
{
    public class CountryRepository : ICountryRepository
    {
        private readonly MovieDbContext _countryContext;

        public CountryRepository(MovieDbContext countryContext)
        {
            _countryContext = countryContext;
        }

        public bool CountryExists(int countryId)
        {
            return _countryContext.Countries.Any(c => c.Id == countryId);
        }

        public ICollection<Country> GetCountries()
        {
            return _countryContext.Countries.OrderBy(c=>c.Name).ToList(); //convert the result to list ToList() to ocnvert IQueryable to ICollection
        }

        public Country GetCountry(int countryId)
        {
            return _countryContext.Countries.Where(c => c.Id == countryId).FirstOrDefault();
        }

        public Country GetCountryOfDirector(int directorId)
        {
            return _countryContext.Directors.Where(d => d.Id == directorId).Select(c => c.Country).FirstOrDefault();
        }

        public ICollection<Director> GetDirectorsFromCountry(int countryId)
        {
            return _countryContext.Directors.Where(d=>d.Country.Id == countryId).ToList();
        }
        public bool IsDuplicateCountryName(int countryId, string countryName)
        {
            var country = _countryContext.Countries.Where(c => c.Id != countryId && c.Name.Trim().ToUpper() == countryName.Trim().ToUpper()).FirstOrDefault();
            return country == null ? false : true;
        }
       
        public bool CreateCountry(Country country)
        {
            _countryContext.AddAsync(country);
            return Save();
        }

        public bool DeleteCountry(Country country)
        {
            _countryContext.Remove(country);
            return Save();
        }

        public bool UpdateCountry(Country country)
        {
            _countryContext.Update(country);
            return Save();
        }
        public bool Save()
        {
            int result = _countryContext.SaveChanges();  //if SaveChanges() > 0, change happened, <0 = something went wrong , =0  no change happened
            return result >= 0 ? true : false;   
        }
    }
}
