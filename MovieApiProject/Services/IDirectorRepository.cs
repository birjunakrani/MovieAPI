using MovieApiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApiProject.Services
{
    public interface IDirectorRepository
    {
        ICollection<Director> GetDirectors();
        Director GetDirector(int directorId);
        ICollection<Director> GetDirectorsOfMovie(int movieId);
        ICollection<Movie> GetMoviesByDirector(int directorId);
        bool DirectorExists(int directorId);
        bool CreateDirector(Director director);
        bool UpdateDirector(Director director);
        bool DeleteDirector(Director director);
        bool Save();

    }
}
