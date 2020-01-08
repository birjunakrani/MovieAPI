using Microsoft.EntityFrameworkCore;
using MovieApiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApiProject.Services
{
    public static class DbSeeding
    {
        public static void seedDataContext(this MovieDbContext context)
        {
            if(!context.MovieDirectors.Any())
            {
               //using MovieDirector associative model class so that can feed data in all tables at once
                var moviesDirectors = new List<MovieDirector>()
                {
                    new MovieDirector()
                    {
                        Movie = new Movie()
                        {
                            Isan = "123",
                            Title = "Three Stooges",
                            DateReleased = new DateTime(2000,4,24),
                            MovieCategories = new List<MovieCategory>()
                            {
                                new MovieCategory { Category = new Category() { Name = "Comedy"}}
                            },
                            Reviews = new List<Review>()
                            {
                                new Review { Headline = "Awesome", ReviewMovie = "The Three Stooges is both loving and peculiar.", Rating = 5,
                                 Critic = new Critic(){ FirstName = "Bob", LastName = "Peters" } },
                                new Review { Headline = "Nofor kids", ReviewMovie = "because theirs some Adult humor", Rating = 4,
                                 Critic = new Critic(){ FirstName = "Peter", LastName = "Griffin" } },
                                new Review { Headline = "Decent", ReviewMovie = "Not a bad read, I kind of liked it", Rating = 3,
                                 Critic = new Critic(){ FirstName = "Paul", LastName = "Griffin" } }
                            }
                        },
                        Director = new Director()
                        {
                            FirstName = "Peter",
                            LastName = "Farrelly",
                            Country = new Country()
                            {
                                Name = "USA"
                            }
                        }
                    },
                    new MovieDirector()
                    {
                        Movie = new Movie()
                        {
                            Isan = "1234",
                            Title = "Skyfall",
                            DateReleased = new DateTime(2012,9,9),
                            MovieCategories = new List<MovieCategory>()
                            {
                                new MovieCategory { Category = new Category() { Name = "Action"}}
                            },
                            Reviews = new List<Review>()
                            {
                                new Review { Headline = "Enjoyed", ReviewMovie = "long time since I enjoyed a Bond movie so much.", Rating = 4,
                                 Critic = new Critic(){ FirstName = "Tom", LastName = "Charity" } }
                            }
                        },
                        Director = new Director()
                        {
                            FirstName = "Sam",
                            LastName = "Mandes",
                            Country = new Country()
                            {
                                Name = "UK"
                            }
                        }
                    },
                    new MovieDirector()
                    {
                        Movie = new Movie()
                        {
                            Isan = "12345",
                            Title = "The Ring",
                            DateReleased = new DateTime(2018,10,18),
                            MovieCategories = new List<MovieCategory>()
                            {
                                new MovieCategory { Category = new Category() { Name = "Horror"}},
                                new MovieCategory { Category = new Category() { Name = "Thriller"}}
                            },
                            Reviews = new List<Review>()
                            {
                                new Review { Headline = "Awesome", ReviewMovie = "Director Gore Verbinski does an excellent job", Rating = 5,
                                Critic = new Critic(){ FirstName = "Nell", LastName = "Minow" } }
                            }
                        },
                        Director = new Director()
                        {
                            FirstName = "Gore",
                            LastName = "Verbinski",
                            Country = new Country()
                            {
                                Name = "France"
                            }
                        }
                    },
                    new MovieDirector()
                    {
                        Movie = new Movie()
                        {
                            Isan = "123456",
                            Title = "Three Musketeers",
                            DateReleased = new DateTime(2011,10,21),
                            MovieCategories = new List<MovieCategory>()
                            {
                                new MovieCategory { Category = new Category() { Name = "Action"}},
                                new MovieCategory { Category = new Category() { Name = "Adventure"}}
                            }
                        },
                        Director = new Director()
                        {
                            FirstName = "Paul",
                            LastName = "Anderson",
                            Country = new Country()
                            {
                                Name = "Germany"
                            }
                        }
                    },
                    new MovieDirector()
                    {
                        Movie = new Movie()
                        {
                            Isan = "1234567",
                            Title = "The NoteBook",
                            DateReleased = new DateTime(2004,6,25),
                            MovieCategories = new List<MovieCategory>()
                            {
                                new MovieCategory { Category = new Category() { Name = "Romance"}}
                            },
                            Reviews = new List<Review>()
                            {
                                new Review { Headline = "Romantic", ReviewMovie = "This movie made me cry a few times", Rating = 5,
                                 Critic = new Critic(){ FirstName = "Allison", LastName = "Kutz" } },
                                new Review { Headline = "Horrible", ReviewMovie = "My wife made me watch it and I hated it", Rating = 1,
                                 Critic = new Critic(){ FirstName = "Kyle", LastName = "Kutz" } }
                            }
                        },
                        Director = new Director()
                        {
                            FirstName = "Nick",
                            LastName = "Cassavettes",
                            Country = new Country()
                            {
                                Name = "Canada"
                            }
                        }
                    }
                };
                context.MovieDirectors.AddRange(moviesDirectors);
                context.SaveChanges();
            } //if
        }//Method
    }//class
}//namespace
