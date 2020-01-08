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
    [Route("//api/[controller]")]
    [ApiController]
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IMovieRepository movieRepository;

        public CategoriesController(ICategoryRepository categoryRepository, IMovieRepository movieRepository)
        {
            this.categoryRepository = categoryRepository;
            this.movieRepository = movieRepository;
        }

        //api/categories
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryDTO>))]
        public IActionResult GetCategories()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var categories = categoryRepository.GetCategories();
            var categoriesDTO = new List<CategoryDTO>();

            foreach (var category in categories)
            {
                var categoryDTO = new CategoryDTO()
                {
                    Id = category.Id,
                    Name = category.Name
                };
                categoriesDTO.Add(categoryDTO);
            }

            return Ok(categoriesDTO);
        }

        //api/categories/{categoryId}
        [HttpGet("{categoryId}",Name="GetCategory")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(CategoryDTO))]
        public IActionResult GetCategory(int categoryId)
        {
            if (!categoryRepository.CategoryExists(categoryId))
            {
                return NotFound();
            }
            var category = categoryRepository.GetCategory(categoryId);

            var categoryDTO = new CategoryDTO()
            {
                Id = category.Id,
                Name = category.Name
            };

            return Ok(categoryDTO);
        }

        //api/categories/movies/{movieId}
        [HttpGet("movies/{movieId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryDTO>))]
        public IActionResult GetCategoriesOfMovie(int movieId)
        {
            if (!movieRepository.MovieExists(movieId))
            {
                return NotFound();
            }

            var categories = categoryRepository.GetCategoriesOfMovie(movieId);
            var categoriesDTO = new List<CategoryDTO>();

            foreach (var category in categories)
            {
                var categoryDTO = new CategoryDTO()
                {
                    Id = category.Id,
                    Name = category.Name
                };
                categoriesDTO.Add(categoryDTO);
            }
            return Ok(categoriesDTO);
        }

        //api/categories/{categoryId}/movies
        [HttpGet("{categoryId}/movies")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MovieDTO>))]
        public IActionResult GetMoviesForCategory(int categoryId)
        {
            if (!categoryRepository.CategoryExists(categoryId))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var movies = categoryRepository.GetMoviesForCategory(categoryId);
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

        //api/categories
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        [ProducesResponseType(201, Type = typeof(Category))]
        public IActionResult CreateCategory([FromBody]Category createCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = categoryRepository.GetCategories().Where(c => c.Name.Trim().ToUpper() == createCategory.Name.Trim().ToUpper()).FirstOrDefault();

            if (category != null)
            {
                ModelState.AddModelError("", $"Category Name {createCategory.Name} already exists");
                return StatusCode(422, ModelState);
            }
            if (!categoryRepository.CreateCategory(createCategory))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetCategory", new { countryId = createCategory.Id }, createCategory);
        }

        //api/categories/{categoryId}
        [HttpPut("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public IActionResult UpdateCategory(int categoryId, [FromBody]Category updateCategory)
        {
            if (updateCategory==null && categoryId != updateCategory.Id && !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(categoryRepository.IsDuplicateCategoryName(categoryId, updateCategory.Name))
            { 
                ModelState.AddModelError("", $"Category Name {updateCategory.Name} already exists");
                return StatusCode(422, ModelState);
            }

            if (!categoryRepository.UpdateCategory(updateCategory))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        //api/categories/{categoryId}
        [HttpDelete("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public IActionResult DeleteCategory(int categoryId)
        {
            var deleteCategory = categoryRepository.GetCategory(categoryId);

            if (!categoryRepository.CategoryExists(categoryId))
            {
                return NotFound();
            }

            if (categoryRepository.GetMoviesForCategory(categoryId).Count() > 0)
            {
                ModelState.AddModelError("", "There are movies connected with this table");
                return StatusCode(409, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!categoryRepository.DeleteCategory(deleteCategory))
            {
                ModelState.AddModelError("", "Something went wrong, please try again");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

    }
}
