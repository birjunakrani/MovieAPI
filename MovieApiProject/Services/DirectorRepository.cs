using MovieApiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApiProject.Services
{
    public class DirectorRepository : IDirectorRepository
    {
        private readonly MovieDbContext _directorContext;

        public DirectorRepository(MovieDbContext directorContext)
        {
            _directorContext = directorContext;
        }

        public bool DirectorExists(int directorId)
        {
            return _directorContext.Directors.Any(d => d.Id == directorId);
        }
        public ICollection<Director> GetDirectors()
        {
            return _directorContext.Directors.OrderBy(d => d.LastName).ToList();
        }

        public Director GetDirector(int directorId)
        {
            return _directorContext.Directors.Where(d => d.Id == directorId).FirstOrDefault();
        }

         public ICollection<Director> GetDirectorsOfMovie(int movieId)
        {
            return _directorContext.MovieDirectors.Where(md => md.Movie.Id == movieId).Select(d => d.Director).ToList();
        }

        public ICollection<Movie> GetMoviesByDirector(int directorId)
        {
            return _directorContext.MovieDirectors.Where(md => md.Director.Id == directorId).Select(m => m.Movie).ToList();
        }

        public bool CreateDirector(Director director)
        {
            _directorContext.AddAsync(director);
            return Save();
        }

        public bool UpdateDirector(Director director)
        {
            _directorContext.Update(director);
            return Save();
        }

        public bool DeleteDirector(Director director)
        {
            _directorContext.Remove(director);
            return Save();
        }

        public bool Save()
        {
            int result = _directorContext.SaveChanges(); //if SaveChanges() > 0, change happened, <0 = something went wrong , =0  no change happened
            return result > 0 ? true : false; throw new NotImplementedException();
        }
    }
}
