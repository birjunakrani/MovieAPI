using Microsoft.AspNetCore.Mvc;
using MovieApiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApiProject.Services
{
    
    public class CategoryRepository : ICategoryRepository
    {
        private readonly MovieDbContext _categoryContext;

        public CategoryRepository(MovieDbContext categoryContext)
        {
            _categoryContext = categoryContext;
        }
        public bool CategoryExists(int categoryId)
        {
            return _categoryContext.Categories.Any(c=>c.Id == categoryId);
        }

        public ICollection<Category> GetCategories()
        {
            return _categoryContext.Categories.OrderBy(c => c.Name).ToList();
        }

        public Category GetCategory(int categoryId)
        {
            return _categoryContext.Categories.Where(c => c.Id == categoryId).FirstOrDefault();
        }

        public ICollection<Category> GetCategoriesOfMovie(int movieId)
        {
                return _categoryContext.MovieCategories.Where(mc => mc.MovieId == movieId).Select(c => c.Category).ToList();
        }

        public ICollection<Movie> GetMoviesForCategory(int categoryId)
        {
            return _categoryContext.MovieCategories.Where(mc => mc.CategoryId == categoryId).Select(m => m.Movie).ToList();
        }

        public bool IsDuplicateCategoryName(int categoryId, string categoryName)
        {
            var category = _categoryContext.Categories.Where(c => c.Name.Trim().ToUpper() == categoryName.Trim().ToUpper() && c.Id != categoryId).FirstOrDefault();
            return category == null ? false : true;
        }

        public bool CreateCategory(Category category)
        {
            _categoryContext.AddAsync(category);
            return Save();
        }

        public bool UpdateCategory(Category category)
        {
            _categoryContext.Update(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _categoryContext.Remove(category);
            return Save();
        }

        public bool Save()
        {
            int  result = _categoryContext.SaveChanges();
            return result >= 0 ? true : false;  //if SaveChanges() > 0, change happened, < 0 = something went wrong, = 0 then no change happened

        }
    }
}
