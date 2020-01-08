using MovieApiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApiProject.Services
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MovieDbContext _movieContext;

        public MovieRepository(MovieDbContext movieContext )
        {
            _movieContext = movieContext;
        }
        public ICollection<Movie> GetMovies()
        {
            return _movieContext.Movies.OrderBy(m=>m.Title).ToList();
        }

        public Movie GetMovie(int movieId)
        {
            return _movieContext.Movies.Where(m => m.Id == movieId).FirstOrDefault();
        }

        public Movie GetMovie(string Isan)
        {
            return _movieContext.Movies.Where(m => m.Isan == Isan).FirstOrDefault();
        }
        public decimal GetMovieRating(int movieId) //calcuate the average rating
        {
            var reviews = _movieContext.Reviews.Where(r => r.Movie.Id == movieId); //get all reviews from review table matching Id of the movie
            if(reviews.Count() <= 0)
            {
                return 0;
            }
            return ((decimal)reviews.Sum(r => r.Rating) / reviews.Count());
        }

        public bool IsDuplicateIsan(int movieId, string Isan)
        {
            var movie = _movieContext.Movies.Where(m => m.Isan.Trim().ToUpper() == Isan .Trim().ToUpper() && m.Id != movieId).FirstOrDefault();
            return movie == null ? false : true;
        }
       
        public bool MovieExists(int movieId)
        {
            return _movieContext.Movies.Any(m => m.Id == movieId);
        }

        public bool MovieExists(string Isan)
        {
            return _movieContext.Movies.Any(m => m.Isan == Isan);
        }

        //need Lis<int> as a paarameted as these list of ids coming from Uri
        public bool CreateMovie(List<int> directorId, List<int> categoryId, Movie movie)
        {
            //Movie enittiy model also has reeference to director & category model
            var directors = _movieContext.Directors.Where(d => directorId.Contains(d.Id)).ToList();
            var categories = _movieContext.Categories.Where(c => categoryId.Contains(c.Id)).ToList();
            
            foreach(var director in directors)
            {
                var movieDirector = new MovieDirector()
                {
                    Movie = movie,
                    Director = director
                };
                _movieContext.Add(movieDirector);
            }

            foreach (var category in categories)
            {
                var movieCategory = new MovieCategory()
                {
                    Movie = movie,
                    Category = category
                };
                _movieContext.Add(movieCategory);
            }
            _movieContext.Add(movie);

            return Save();
        }

        public bool UpdateMovie(List<int> directorId, List<int> categoryId, Movie movie)
        {
            var directors = _movieContext.Directors.Where(d => directorId.Contains(d.Id)).ToList();
            var categories = _movieContext.Categories.Where(c => categoryId.Contains(c.Id)).ToList();

            var deleteMovieDirector = _movieContext.MovieDirectors.Where(md => md.MovieId == movie.Id);
            var deleteMovieCategory = _movieContext.MovieCategories.Where(mc => mc.MovieId == movie.Id);

            _movieContext.RemoveRange(deleteMovieDirector);
            _movieContext.RemoveRange(deleteMovieCategory);

            foreach (var director in directors)
            {
                var movieDirector = new MovieDirector()
                {
                    Movie = movie,
                    Director = director
                };
                _movieContext.Add(movieDirector);
            }

            foreach (var category in categories)
            {
                var movieCategory = new MovieCategory()
                {
                    Movie = movie,
                    Category = category
                };
                _movieContext.Add(movieCategory);
            }
            _movieContext.Update(movie);

            return Save();
        }

        public bool DeleteMovie(Movie movie)
        {
            _movieContext.Remove(movie);
            return Save();
        }

        public bool Save()
        {
            int result = _movieContext.SaveChanges(); //if SaveChanges() > 0, change happened, <0 = something went wrong , =0  no change happened
            return result > 0 ? true : false;
        }
    }
}
