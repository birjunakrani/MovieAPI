using MovieApiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApiProject.Services
{
    public interface ICriticRepository
    {
        ICollection<Critic> GetCritics();
        Critic GetCritic(int criticId);
        ICollection<Review> GetReviewsByCritic(int criticId);
        Critic GetCriticOfReview(int reviewId);
        bool CriticExists(int criticId);
        bool CreateCritic(Critic critic);
        bool UpdateCritic(Critic critic);
        bool DeleteCritic(Critic critic);
        bool Save();
    }
}
