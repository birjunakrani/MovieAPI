using MovieApiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApiProject.Services
{
    public interface IReviewRepository
    {
        ICollection<Review> GetReviews();
        Review GetReview(int reviewId);
        ICollection<Review> GetReviewsOfMovie(int movieId);
        Movie GetMovieOfReview(int reviewId);
        Critic GetCriticOfReview(int reviewId);
        bool ReviewExists(int revieId);
        bool CreateReview(Review review);
        bool UpdateReview(Review review);
        bool DeleteReview(Review review);

        //as list of reviews are associated with each Critic,parent-child relationship, need to delete childeren(reviews) if deleting parent(Critic) and this method will thus be called while deleting critic at critic controller
        bool DeleteReviews(List<Review> reviews);
        bool Save();
    }
}
