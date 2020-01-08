using MovieApiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApiProject.Services
{
    public class CriticRepository : ICriticRepository
    {
        private readonly MovieDbContext _criticContext;
        private readonly ReviewRepository reviewRepository;

        public CriticRepository(MovieDbContext criticContext)
        {
            _criticContext = criticContext;
        }
        public bool CriticExists(int criticId)
        {
            return _criticContext.Critics.Any(c => c.Id == criticId);
        }
        public ICollection<Critic> GetCritics()
        {
            return _criticContext.Critics.ToList();
        }

        public Critic GetCritic(int criticId)
        {
            return _criticContext.Critics.Where(c => c.Id == criticId).FirstOrDefault();
        }

        public Critic GetCriticOfReview(int reviewId)
        {
            return _criticContext.Reviews.Where(r => r.Id == reviewId).Select(c => c.Critic).FirstOrDefault();
        }
        public ICollection<Review> GetReviewsByCritic(int criticId)
        {
            return _criticContext.Reviews.Where(r => r.Critic.Id == criticId).ToList();
        }

        public bool CreateCritic(Critic critic)
        {
            _criticContext.AddAsync(critic);
            return Save();
        }

        public bool UpdateCritic(Critic critic)
        {
            _criticContext.Update(critic);
            return Save();
        }

        public bool DeleteCritic(Critic critic)
        {
            _criticContext.Remove(critic);
            return Save();
        }

        public bool Save()
        {
            int result = _criticContext.SaveChanges(); //if SaveChanges() > 0, change happened, <0 = something went wrong , =0  no change happened
            return result>0 ? true : false;
        }
    }
}
