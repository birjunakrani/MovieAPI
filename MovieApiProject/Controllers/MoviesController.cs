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
    public class MoviesController : Controller
    {
        private readonly IMovieRepository movieRepository;
        private readonly IDirectorRepository directorRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IReviewRepository reviewRepository;

        public MoviesController(IMovieRepository movieRepository, IDirectorRepository directorRepository, ICategoryRepository categoryRepository, IReviewRepository reviewRepository)
        {
            this.movieRepository = movieRepository;
            this.directorRepository = directorRepository;
            this.categoryRepository = categoryRepository;
            this.reviewRepository = reviewRepository;
        }

        //api/movies
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MovieDTO>))]
        public IActionResult GetMovies()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var movies = movieRepository.GetMovies();
            var moviesDTO = new List<MovieDTO>();

            foreach (var movie in movies)
            {
                moviesDTO.Add(new MovieDTO
                {
                    Id = movie.Id,
                    Isan = movie.Isan,
                    Title = movie.Title,
                    DateReleased = movie.DateReleased
                });
            }
            return Ok(moviesDTO);
        }

        //api/movies/{movieId}
        [HttpGet("{movieId}", Name = "GetMovie")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(MovieDTO))]
        public IActionResult GetMovie(int movieId)
        {
            if (!movieRepository.MovieExists(movieId))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var movie = movieRepository.GetMovie(movieId);
            var movieDTO = new MovieDTO
            {
                Id = movie.Id,
                Isan = movie.Isan,
                Title = movie.Title,
                DateReleased = movie.DateReleased
            };
            return Ok(movieDTO);
        }

        //api/movies/ISAN/{Isan}
        [HttpGet("ISAN/{Isan}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(MovieDTO))]
        public IActionResult GetMovie(string Isan)
        {
            if (!movieRepository.MovieExists(Isan))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var movie = movieRepository.GetMovie(Isan);
            var movieDTO = new MovieDTO
            {
                Id = movie.Id,
                Isan = movie.Isan,
                Title = movie.Title,
                DateReleased = movie.DateReleased
            };
            return Ok(movieDTO);
        }

        //api/movies/{movieId}/rating
        [HttpGet("{movieId}/rating")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(MovieDTO))]
        public IActionResult GetMovieRating(int movieId)
        {
            if (!movieRepository.MovieExists(movieId))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var rating = movieRepository.GetMovieRating(movieId);

            return Ok(rating);
        }

        //.api/movies?directorId=xx&directorId=xx&catId=x&catId=x
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        [ProducesResponseType(201, Type=typeof(Movie))]
        public IActionResult CreateMovie([FromQuery]List<int> dirId, [FromQuery]List<int> catId, [FromBody]Movie createMovie)
        {
            var statusCode = ValidateMovie(dirId, catId, createMovie);
            if (!ModelState.IsValid)
            {
                return StatusCode(statusCode.StatusCode);
            }

            if (!movieRepository.CreateMovie(dirId, catId, createMovie))
            {
                ModelState.AddModelError("", "Something went wrong, Please try again");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetMovie", new { movieId = createMovie.Id }, createMovie);
        }


        //.api/movies/movieId?directorId=xx&directorId=xx&catId=x&catId=x
        [HttpPut("{movieId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public IActionResult UpdateMovie(int movieId,[FromQuery]List<int> dirId, [FromQuery]List<int> catId, [FromBody]Movie updateMovie)
        {
            var statusCode = ValidateMovie(dirId, catId, updateMovie);
            if(movieId != updateMovie.Id)
            {
                return BadRequest();
            }
            if (movieRepository.MovieExists(movieId))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return StatusCode(statusCode.StatusCode);
            }

            if (!movieRepository.UpdateMovie(dirId, catId, updateMovie))
            {
                ModelState.AddModelError("", "Something went wrong, Please try again");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        //.api/movies/movieId
        [HttpPut("{movieId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public IActionResult DeleteMovie(int movieId)
        {
            if (movieRepository.MovieExists(movieId))
            {
                return NotFound();
            }

            var deleteMovie = movieRepository.GetMovie(movieId);
            var deleteReviews = reviewRepository.GetReviewsOfMovie(movieId);

            if (!movieRepository.DeleteMovie(deleteMovie) || !reviewRepository.DeleteReviews(deleteReviews.ToList()))
            {
                ModelState.AddModelError("", "Something went wrong, Please try again");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }


        private StatusCodeResult ValidateMovie(List<int> dirId, List<int> catId, Movie movie)
        {
            if(movie==null || dirId.Count()<=0 || catId.Count() <= 0)
            {
                ModelState.AddModelError("", "Either movie, category or director doesn't exists");
                return BadRequest();
            }
            if (movieRepository.IsDuplicateIsan(movie.Id, movie.Isan))
            {
                ModelState.AddModelError("", "Isan already exists");
                return StatusCode(422);
            }
            foreach(int id in dirId)
            {
                if (!directorRepository.DirectorExists(id))
                {
                    ModelState.AddModelError("", "directorId Not Found");
                    return StatusCode(404);
                }
            }
            foreach (int id in catId)
            {
                if (!categoryRepository.CategoryExists(id))
                {
                    ModelState.AddModelError("", "categoryId Not Found");
                    return StatusCode(404);
                }
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return NoContent();
        }
    }
}
