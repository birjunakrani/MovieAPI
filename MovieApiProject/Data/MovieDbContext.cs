using Microsoft.EntityFrameworkCore;
using MovieApiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApiProject.Services
{
    public class MovieDbContext : DbContext
    {
        public MovieDbContext(DbContextOptions<MovieDbContext> dbContextoptions) : base(dbContextoptions)
        {}

        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<Director> Directors {get; set;}
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Critic> Critics { get; set; }
        public virtual DbSet<MovieDirector> MovieDirectors { get; set; }
        public virtual DbSet<MovieCategory> MovieCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //establishing many-many relationship in DB
            modelBuilder.Entity<MovieCategory>()
                        .HasKey(mc => new { mc.MovieId, mc.CategoryId });

            modelBuilder.Entity<MovieCategory>()
                        .HasOne(m => m.Movie)
                        .WithMany(mc => mc.MovieCategories)
                        .HasForeignKey(m => m.MovieId);

            modelBuilder.Entity<MovieCategory>()
                       .HasOne(c => c.Category)
                       .WithMany(mc => mc.MovieCategories)
                       .HasForeignKey(c => c.CategoryId);

            modelBuilder.Entity<MovieDirector>()
                        .HasKey(md => new { md.MovieId, md.DirectorId });

            modelBuilder.Entity<MovieDirector>()
                      .HasOne(m => m.Movie)
                      .WithMany(md => md.MovieDirectors)
                      .HasForeignKey(m => m.MovieId);

            modelBuilder.Entity<MovieDirector>()
                   .HasOne(d => d.Director)
                   .WithMany(md => md.MovieDirectors)
                   .HasForeignKey(d => d.DirectorId);
        }
    }
}
