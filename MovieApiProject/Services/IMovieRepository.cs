using MovieApiProject.Models;
using System.Collections.Generic;

namespace MovieApiProject.Services
{
    public interface IMovieRepository
    {
        ICollection<Movie> GetMovies();
        Movie GetMovie(int movieId);
        Movie GetMovie(string Isan);
        decimal GetMovieRating(int movieId);
        bool IsDuplicateIsan(int movieId, string Isan);
        bool MovieExists(int movieId);
        bool MovieExists(string Isan);
        bool CreateMovie(List<int> directorsId, List<int> categoriesId, Movie movie);
        bool UpdateMovie(List<int> directorsId, List<int> categoriesId, Movie movie);
        bool DeleteMovie(Movie movie);
      
        bool Save();



    }
}