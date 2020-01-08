using MovieApiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApiProject.Services
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly MovieDbContext _reviewContext;

        public bool ReviewExists(int revieId)
        {
            return _reviewContext.Reviews.Any(r => r.Id == revieId);
        }


        public ReviewRepository(MovieDbContext reviewContext)
        {
            _reviewContext = reviewContext;
        }
        public ICollection<Review> GetReviews()
        {
            return _reviewContext.Reviews.ToList();
        }

        public Review GetReview(int reviewId)
        {
            return _reviewContext.Reviews.Where(r => r.Id == reviewId).FirstOrDefault();
        }

        public Movie GetMovieOfReview(int reviewId)
        {
            return _reviewContext.Reviews.Where(r => r.Id == reviewId).Select(m => m.Movie).FirstOrDefault();
        }

        public ICollection<Review> GetReviewsOfMovie(int movieId)
        {
            return _reviewContext.Reviews.Where(r => r.Movie.Id == movieId).ToList();
        }

        public Critic GetCriticOfReview(int reviewId)
        {
            return _reviewContext.Reviews.Where(r => r.Id == reviewId).Select(c=>c.Critic).FirstOrDefault();
        }

        public bool CreateReview(Review review)
        {
            _reviewContext.AddAsync(review);
            return Save();
        }

        public bool UpdateReview(Review review)
        {
            _reviewContext.Update(review);
            return Save();
        }

        public bool DeleteReview(Review review)
        {
            _reviewContext.Remove(review);
            return Save();
        }
        public bool DeleteReviews(List<Review> reviews)
        {
            _reviewContext.RemoveRange(reviews);
            return Save();
        }
        public bool Save()
        {
            var result = _reviewContext.SaveChanges(); //if SaveChanges() > 0, change happened, <0 = something went wrong , =0  no change happened
            return result > 0 ? true : false;
        }
    }
}
