using Microsoft.AspNetCore.Mvc;
using MovieApiProject.DTO;
using MovieApiProject.Models;
using MovieApiProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApiProject.Controllers
{
    [Route("//api/[Controller]")] //Route attribure to define route api with dynamic controller
    [ApiController] //this annotation indicates the controller to be API controller
    public class CountriesController : Controller
    {
        private readonly ICountryRepository countryRepository;

        public CountriesController(ICountryRepository countryRepository)
        {
            this.countryRepository = countryRepository;
        }

        //api/countries
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200,Type = typeof(IEnumerable<CountryDTO>))]
        public IActionResult GetCountries()
        {
            var countries = countryRepository.GetCountries().ToList();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var countriesDTO = new List<CountryDTO>();
            foreach(var country in countries)
            {
                countriesDTO.Add(new CountryDTO
                {
                    Id = country.Id,
                    Name = country.Name
                });
            }
            return Ok(countriesDTO);
        }

        //api/countries/countryId
        [HttpGet("{countryId}",Name="GetCountry")] //adding route parameter to the route
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(CountryDTO))]
        public IActionResult GetCountry(int countryId)
        {
            if (!countryRepository.CountryExists(countryId))
            {
                return NotFound();
            }
            var country = countryRepository.GetCountry(countryId);
            var countryDTO = new CountryDTO()
            {
                Id = country.Id,
                Name = country.Name
            };
            return Ok(countryDTO);
        }

        //api/countries/directors/directorId
        [HttpGet("directors/{directorId}")] //adding route parameter to the route
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(CountryDTO))]
        public IActionResult GetCountryOfDirector(int directorId)
        {
            //TO do - validate author exists
            var country = countryRepository.GetCountryOfDirector(directorId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var countryDTO = new CountryDTO()
            {
                Id = country.Id,
                Name = country.Name
            };
            return Ok(countryDTO);
        }

        //api/countries
        [HttpPost]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        [ProducesResponseType(201, Type=typeof(Country))]
        public IActionResult CreateCountry([FromBody]Country createCountry) //FromBody indicates the object is coming from body/form
        {
            if(createCountry == null)
            {
                return BadRequest(ModelState);
            }

            //to avoid duplication entry ofthe country Name
            var country = countryRepository.GetCountries().Where(c => c.Name.Trim().ToUpper() == createCountry.Name.Trim().ToUpper()).FirstOrDefault();
            if (country != null)
            {
                ModelState.AddModelError("", "Country you are trying to enter already exists");
                return StatusCode(422, $"Country {country.Name} you are trying to enter already exists");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!countryRepository.CreateCountry(createCountry))
            {
                ModelState.AddModelError("", "Something Went wrong");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetCountry", new { countryId = createCountry.Id },createCountry);
        }
        
        //api/countries/{countryId}
        [HttpPut("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public IActionResult UpdateCountry(int countryId, [FromBody]Country updateCountry)
        {
            if(updateCountry == null || countryId!=updateCountry.Id && !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!countryRepository.CountryExists(countryId))
            {
                return NotFound();
            }
            if (countryRepository.IsDuplicateCountryName(countryId, updateCountry.Name))
            {
                ModelState.AddModelError("", $"country {updateCountry.Name} you are trying to enter already exists");
                return StatusCode(422, ModelState);
            }
            if (!countryRepository.UpdateCountry(updateCountry))
            {
                ModelState.AddModelError("", "Something went wrong, please try again");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        //api/countries/{countryId}
        [HttpDelete("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public IActionResult DeleteCountry(int countryId)
        {
            if (!countryRepository.CountryExists(countryId))
            {
                return NotFound();
            }
            var deleteContry = countryRepository.GetCountry(countryId);

            if (countryRepository.GetDirectorsFromCountry(countryId).Count > 0)
            {
                ModelState.AddModelError("", "Country can not be deleted as One or More directors are still connnected");
                return StatusCode(409, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!countryRepository.DeleteCountry(deleteContry))
            {
                ModelState.AddModelError("", $"Something went wrong, please try again");
            }
            return NoContent();
        }
    }
}
