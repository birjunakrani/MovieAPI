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
    public class ReviewsController : Controller
    {
        private readonly IReviewRepository reviewRepository;
        private readonly IMovieRepository movieRepository;
        private readonly ICriticRepository criticRepository;

        public ReviewsController(IReviewRepository reviewRepository, IMovieRepository movieRepository, ICriticRepository criticRepository)
        {
            this.reviewRepository = reviewRepository;
            this.movieRepository = movieRepository;
            this.criticRepository = criticRepository;
        }

        //api/reviews
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDTO>))]
        public IActionResult GetReviews()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var reviews = reviewRepository.GetReviews();
            var reviewsDTO = new List<ReviewDTO>();
            foreach (var review in reviews)
            {
                var reviewDTO = new ReviewDTO()
                {
                    Id = review.Id,
                    Headline = review.Headline,
                    ReviewMovie = review.ReviewMovie,
                    Rating = review.Rating
                };
                reviewsDTO.Add(reviewDTO);
            }
            return Ok(reviewsDTO);
        }

        //api/reviews/{reviewId}
        [HttpGet("{reviewId}", Name = "GetReview")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(ReviewDTO))]
        public IActionResult GetReview(int reviewId)
        {
            if (!reviewRepository.ReviewExists(reviewId))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var review = reviewRepository.GetReview(reviewId);
            var reviewDTO = new ReviewDTO()
            {
                Id = review.Id,
                Headline = review.Headline,
                ReviewMovie = review.ReviewMovie,
                Rating = review.Rating
            };
            return Ok(reviewDTO);
        }

        //api/reviews/{reviewId}/movies
        [HttpGet("{reviewId}/movies")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(MovieDTO))]
        public IActionResult GetMovieOfReview(int reviewId)
        {
            if (!reviewRepository.ReviewExists(reviewId))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var movie = reviewRepository.GetMovieOfReview(reviewId);
            var movieDTO = new MovieDTO()
            {
                Id = movie.Id,
                Isan = movie.Isan,
                Title = movie.Title,
                DateReleased = movie.DateReleased
            };
            return Ok(movieDTO);
        }

        //api/reviews/{reviewId}/critics
        [HttpGet("{reviewId}/critics")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(CriticDTO))]
        public IActionResult GetCriticOfReview(int reviewId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!reviewRepository.ReviewExists(reviewId))
            {
                return NotFound();
            }
            var critic = reviewRepository.GetCriticOfReview(reviewId);
            var criticDTO = new CriticDTO()
            {
                Id = critic.Id,
                FirstName = critic.FirstName,
                LastName = critic.LastName
            };
            return Ok(criticDTO);
        }

        //api/reviews/movies/{movieId}
        [HttpGet("movies/{movieId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDTO>))]
        public IActionResult GetReviewsOfMovie(int movieId)
        {
            // TO DO - check if movieId exists

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var reviews = reviewRepository.GetReviewsOfMovie(movieId);
            var reviewsDTO = new List<ReviewDTO>();
            foreach (var review in reviews)
            {
                var reviewDTO = new ReviewDTO()
                {
                    Id = review.Id,
                    Headline = review.Headline,
                    ReviewMovie = review.ReviewMovie,
                    Rating = review.Rating
                };
                reviewsDTO.Add(reviewDTO);
            }
            return Ok(reviewsDTO);
        }

        //api/reviews
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesResponseType(201, Type = typeof(Review))]
        public IActionResult CreateReview([FromBody]Review createReview)
        {
            if (createReview == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //As Review entity model also has reference to Moview & Critic object due to entity relationship, need to populate these both objects

            if (!criticRepository.CriticExists(createReview.Critic.Id) || !movieRepository.MovieExists(createReview.Movie.Id)) //Id comes from View as hidden input when user selects Critic/movie from e.g. dropdown
            {
                ModelState.AddModelError("", "Critic or Movie doesn't exists for this review");
            }
           
            if (!ModelState.IsValid)
            {
                return StatusCode(404, ModelState);
            }
            createReview.Movie = movieRepository.GetMovie(createReview.Movie.Id); //Id comes from View as hidden input when user selects movie from e.g. dropdown
            createReview.Critic = criticRepository.GetCritic(createReview.Critic.Id); //Id comes from View as hidden input when user selects critic from e.g. dropdown

            if (!reviewRepository.CreateReview(createReview))
            {
                ModelState.AddModelError("", "Something went wrong, Please try again");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetReview", new { reviewId = createReview.Id }, createReview);
        }

        //api/reviews/reviewId
        [HttpPut("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public IActionResult UpdateReview(int reviewId, [FromBody]Review updateReview)
        {
            if(updateReview==null || reviewId != updateReview.Id || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // As Review entity model also has reference to Moview & Critic object due to entity relationship, need to populate these both objects
            if (!reviewRepository.ReviewExists(reviewId) || !movieRepository.MovieExists(updateReview.Movie.Id) || !criticRepository.CriticExists(updateReview.Critic.Id))
            {
                return NotFound(404);
            }
            if (!ModelState.IsValid)
            {
                return StatusCode(404, ModelState);
            }

            updateReview.Movie = movieRepository.GetMovie(updateReview.Movie.Id); //Id comes from View as hidden input when user selects Critic from e.g. dropdown
            updateReview.Critic = criticRepository.GetCritic(updateReview.Critic.Id); //Id comes from View as hidden input when user selects movie from e.g. dropdown

            if (!reviewRepository.UpdateReview(updateReview))
            {
                ModelState.AddModelError("", $"Something went wrong saving the review");
                return StatusCode(500,ModelState);
            }

            return NoContent();
        }

        //api/reviews/reviewId
        [HttpDelete("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public IActionResult DeleteReview(int reviewId)
        {
            if (!reviewRepository.ReviewExists(reviewId))
            {
                return NotFound(404);
            }

            var deleteReview = reviewRepository.GetReview(reviewId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!reviewRepository.DeleteReview(deleteReview))
            {
                ModelState.AddModelError("", "Something went wrong,Please try again");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}
