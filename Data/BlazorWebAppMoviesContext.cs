using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorWebAppMovies.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorWebAppMovies.Data
{
    public class BlazorWebAppMoviesContext : DbContext
    {
        public BlazorWebAppMoviesContext(DbContextOptions<BlazorWebAppMoviesContext> options)
            : base(options)
        {
        }

        public DbSet<BlazorWebAppMovies.Models.Movie> Movie { get; set; } = default!;
    }
}
