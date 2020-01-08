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
    [ApiController]
    [Route("api/[controller]")]
    public class DirectorsController : Controller
    {
        private readonly IDirectorRepository directorRepository;
        private readonly IMovieRepository movieRepository;
        private readonly ICountryRepository countryRepository;

        public DirectorsController(IDirectorRepository directorRepository, IMovieRepository movieRepository, ICountryRepository countryRepository)
        {
            this.directorRepository = directorRepository;
            this.movieRepository = movieRepository;
            this.countryRepository = countryRepository;
        }

        //api/directors
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200,Type=typeof(IEnumerable<DirectorDTO>))]
        public IActionResult GetDirectors()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var directors = directorRepository.GetDirectors();
            var directorsDTO = new List<DirectorDTO>();
            foreach (var director in directors)
            {
                directorsDTO.Add(new DirectorDTO()
                {
                    Id = director.Id,
                    FirstName = director.FirstName,
                    LastName = director.LastName
                });
            }
            return Ok(directorsDTO);
        }

        //api/directors/{directorId}
        [HttpGet("{directorId}",Name= "GetDirector")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(DirectorDTO))]
        public IActionResult GetDirector(int directorId)
        {
            if (!directorRepository.DirectorExists(directorId))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var director = directorRepository.GetDirector(directorId);
            var directorDTO = new DirectorDTO()
                {
                    Id = director.Id,
                    FirstName = director.FirstName,
                    LastName = director.LastName
                };
            return Ok(directorDTO);
        }

        //api/directors/movie/{movieId}
        [HttpGet("movies/{movieId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<DirectorDTO>))]
        public IActionResult GetDirectorsOfMovie(int movieId)
        {
            if (!movieRepository.MovieExists(movieId))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var directors = directorRepository.GetDirectorsOfMovie(movieId);
            var directorsDTO = new List<DirectorDTO>();
            foreach(var director in directors)
            {
                directorsDTO.Add(new DirectorDTO()
                {
                    Id = director.Id,
                    FirstName = director.FirstName,
                    LastName = director.LastName
                });
            }
            return Ok(directorsDTO);
        }


        //api/directors/{directorId}/movies
        [HttpGet("{directorId}/movies")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MovieDTO>))]
        public IActionResult GetMoviesByDirector(int directorId)
        {
            if (!movieRepository.MovieExists(directorId))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var movies = directorRepository.GetMoviesByDirector(directorId);
            var moviesDTO = new List<MovieDTO>();
            foreach (var movie in movies)
            {
                moviesDTO.Add(new MovieDTO()
                {
                    Id = movie.Id,
                    Isan= movie.Isan,
                    Title = movie.Title,
                    DateReleased = movie.DateReleased
                });
            }
            return Ok(moviesDTO);
        }

        //api/directors
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesResponseType(201, Type = typeof(Director))]
        public IActionResult CreateDirector([FromBody]Director createDirector)
        {
            if (createDirector == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //As Director entity model also has reference to Moview & DIrector object due to entity relationship, need to populate these both objects

            if (!countryRepository.CountryExists(createDirector.Country.Id)) //Id comes from View as hidden input when user selects Country from e.g. dropdown
            {
                ModelState.AddModelError("", "Country doesn't exists");
            }
            if (!ModelState.IsValid)
            {
                return StatusCode(404, ModelState);
            }
            createDirector.Country = countryRepository.GetCountry(createDirector.Country.Id); //Id comes from View as hidden input when user selects Critic from e.g. dropdown
           
            if (!directorRepository.CreateDirector(createDirector))
            {
                ModelState.AddModelError("", "Something went wrong, Please try again");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetDirector", new { directorID = createDirector.Id }, createDirector);
        }

        //api/directors/directorId
        [HttpPut("{directorId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public IActionResult UpdateReview(int directorId, [FromBody]Director updateDirector)
        {
            if (updateDirector == null || directorId != updateDirector.Id || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //As director entity model also has reference to Country object due to entity relationship, need to populate this object
            if (!directorRepository.DirectorExists(directorId) || !countryRepository.CountryExists(updateDirector.Country.Id))
            {
                return NotFound(404);
            }
            if (!ModelState.IsValid)
            {
                return StatusCode(404, ModelState);
            }

            updateDirector.Country = countryRepository.GetCountry(updateDirector.Country.Id); //Id comes from View as hidden input when user selects countrt from e.g. dropdown
            
            if (!directorRepository.UpdateDirector(updateDirector))
            {
                ModelState.AddModelError("", $"Something went wrong , please try again");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        //api/directors/directorId
        [HttpDelete("{directorId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public IActionResult DeleteReview(int directorId)
        {
            if (!directorRepository.DirectorExists(directorId))
            {
                return NotFound(404);
            }

            var deleteReview = directorRepository.GetDirector(directorId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(directorRepository.GetMoviesByDirector(directorId).Count() > 0)
            {
                ModelState.AddModelError("", "there are movies associated and thus can't delete");
                return StatusCode(409, ModelState);
            }
            if (!directorRepository.DeleteDirector(deleteReview))
            { 
                ModelState.AddModelError("", "Something went wrong,Please try again");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

    }
}
