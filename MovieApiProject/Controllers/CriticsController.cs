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
    [Route("api/[controller]")]
    [ApiController]
    public class CriticsController : Controller
    {
        private readonly ICriticRepository criticRepository;
        private readonly IReviewRepository reviewRepository;

        public CriticsController(ICriticRepository criticRepository, IReviewRepository reviewRepository)
        {
            this.criticRepository = criticRepository;
            this.reviewRepository = reviewRepository;
        }

       //api/critics
       [HttpGet]
       [ProducesResponseType(400)]
       [ProducesResponseType(404)]
       [ProducesResponseType(200, Type=typeof(IEnumerable<CriticDTO>))]
       public IActionResult GetCritics()
        {
   
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var critics = criticRepository.GetCritics();
            var criticsDTO = new List<CriticDTO>();
            foreach(var critic in critics)
            {
                var criticDTO = new CriticDTO()
                {
                    Id = critic.Id,
                    FirstName = critic.FirstName,
                    LastName = critic.LastName
                };
                criticsDTO.Add(criticDTO);
            }
            return Ok(criticsDTO);
        }

        //api/critics/{criticId}
        [HttpGet("{criticId}",Name="GetCritic")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200,Type=typeof(CriticDTO))]
        public IActionResult GetCritic(int criticId)
        {
            if (!criticRepository.CriticExists(criticId))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var critic = criticRepository.GetCritic(criticId);
            var criticDTO = new CriticDTO()
            {
                Id = critic.Id,
                FirstName = critic.FirstName,
                LastName = critic.LastName
            };

            return Ok(criticDTO);
        }

        //api/critics/reviews/{reviewId}
        [HttpGet("reviews/{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(CriticDTO))]
        public IActionResult GetCriticsOfReview(int reviewId)
        {
            if (!reviewRepository.ReviewExists(reviewId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var critic = criticRepository.GetCriticOfReview(reviewId);
            var criticDTO = new CriticDTO()
            {
                Id = critic.Id,
                FirstName = critic.FirstName,
                LastName = critic.LastName
            };

            return Ok(criticDTO);
        }

        //api/critics/{criticId}/reviews
        [HttpGet("{criticId}/reviews")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDTO>))]
        public IActionResult GetReviewsByCritic(int criticId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var reviewsDTO = new List<ReviewDTO>();
            var reviews = criticRepository.GetReviewsByCritic(criticId);

            foreach(var review in reviews)
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


        //api/critics
        [HttpPost]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesResponseType(201, Type = typeof(Critic))]
        public IActionResult CreateCritic([FromBody]Critic createCritic) //FromBody indicates the object is coming from body/form
        {
            if (createCritic == null)
            {
                return BadRequest(ModelState);
            }
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!criticRepository.CreateCritic(createCritic))
            {
                ModelState.AddModelError("", "Something Went wrong, Please try again");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetCritic", new { criticId = createCritic.Id }, createCritic);
        }

        //api/critics/{criticId}
        [HttpPut("{criticId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public IActionResult UpdateCritic(int criticId, [FromBody]Critic updateCritic)
        {
            if (updateCritic == null || criticId != updateCritic.Id || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!criticRepository.CriticExists(criticId))
            {
                return NotFound();
            }

            if (!criticRepository.UpdateCritic(updateCritic))
            {
                ModelState.AddModelError("", "Something went wrong, please try again");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        //api/critics/{criticId}
        [HttpDelete("{criticId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public IActionResult DeleteCritic(int criticId)
        {
            if (!criticRepository.CriticExists(criticId))
            {
                return NotFound();
            }
            var deleteCritic = criticRepository.GetCritic(criticId);
            var deleteReviews = criticRepository.GetReviewsByCritic(criticId);

            if (!criticRepository.DeleteCritic(deleteCritic))
            {
                ModelState.AddModelError("", $"Something went wrong, please try again");
                return StatusCode(500, ModelState);
            }
            if (!reviewRepository.DeleteReviews(deleteReviews.ToList()))
            {
                ModelState.AddModelError("", $"Something went wrong while deleting associated reviews from {deleteCritic.FirstName} {deleteCritic.LastName}, please try again");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

    }
}
