using MovieApiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApiProject.Services
{
    public interface ICountryRepository
    {
        ICollection<Country> GetCountries(); //get all countries

        Country GetCountry(int countryId); //get country by id

        Country GetCountryOfDirector(int directorId); //get Country of a Director
        ICollection<Director> GetDirectorsFromCountry(int countryId);
        bool CountryExists(int countryId);

        bool IsDuplicateCountryName(int countryId, string countryName);
        bool CreateCountry(Country country);
        bool UpdateCountry(Country country);
        bool DeleteCountry(Country country);
        bool Save();

    }
}
