using Microsoft.EntityFrameworkCore;
using MovieApiProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApiProject.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly MovieDbContext _db;

        public DbInitializer(MovieDbContext db)
        {
           _db = db;
        }

        public void Initialize()
        {
            //Migrating to Database
            if (_db.Database.GetPendingMigrations().Count() > 0)
            {
                _db.Database.Migrate();
            }
        }
    }
}
