using BlazorWebAppMovies.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorWebAppMovies.Data;

public class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var context = new BlazorWebAppMoviesContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<BlazorWebAppMoviesContext>>());

        if (context == null || context.Movie == null)
        {
            throw new NullReferenceException(
                "Null BlazorWebAppMoviesContext or Movie DbSet");
        }

        if (context.Movie.Any())
        {
            return;
        }

        context.Movie.AddRange(
            new Movie
            {
                Title = "Mad Max",
                ReleaseDate = new DateOnly(1979, 4, 12),
                Genre = "Sci-fi (Cyberpunk)",
                Price = 2.51M,
                Rating = "R"
            },
            new Movie
            {
                Title = "The Road Warrior",
                ReleaseDate = new DateOnly(1981, 12, 24),
                Genre = "Sci-fi (Cyberpunk)",
                Price = 2.78M,
                Rating = "R"
            },
            new Movie
            {
                Title = "Mad Max: Beyond Thunderdome",
                ReleaseDate = new DateOnly(1985, 7, 10),
                Genre = "Sci-fi (Cyberpunk)",
                Price = 3.55M,
                Rating = "PG-13"
            },
            new Movie
            {
                Title = "Mad Max: Fury Road",
                ReleaseDate = new DateOnly(2015, 5, 15),
                Genre = "Sci-fi (Cyberpunk)",
                Price = 8.43M,
                Rating = "R"
            },
            new Movie
            {
                Title = "Furiosa: A Mad Max Saga",
                ReleaseDate = new DateOnly(2024, 5, 24),
                Genre = "Sci-fi (Cyberpunk)",
                Price = 13.49M,
                Rating = "R"
            },
            new Movie
            {
                Title = "Blade Runner",
                ReleaseDate = new DateOnly(1982, 6, 25),
                Genre = "Sci-fi (Neo-noir)",
                Price = 4.12M,
                Rating = "R"
            },
            new Movie
            {
                Title = "Blade Runner 2049",
                ReleaseDate = new DateOnly(2017, 10, 6),
                Genre = "Sci-fi (Neo-noir)",
                Price = 9.35M,
                Rating = "R"
            },
            new Movie
            {
                Title = "Alien",
                ReleaseDate = new DateOnly(1979, 5, 25),
                Genre = "Sci-fi (Horror)",
                Price = 3.78M,
                Rating = "R"
            },
            new Movie
            {
                Title = "Aliens",
                ReleaseDate = new DateOnly(1986, 7, 18),
                Genre = "Sci-fi (Action)",
                Price = 4.56M,
                Rating = "R"
            },
            new Movie
            {
                Title = "Prometheus",
                ReleaseDate = new DateOnly(2012, 6, 8),
                Genre = "Sci-fi (Horror)",
                Price = 6.25M,
                Rating = "R"
            },
            new Movie
            {
                Title = "The Matrix",
                ReleaseDate = new DateOnly(1999, 3, 31),
                Genre = "Sci-fi (Action)",
                Price = 5.48M,
                Rating = "R"
            },
            new Movie
            {
                Title = "The Matrix Reloaded",
                ReleaseDate = new DateOnly(2003, 5, 15),
                Genre = "Sci-fi (Action)",
                Price = 5.97M,
                Rating = "R"
            },
            new Movie
            {
                Title = "The Matrix Revolutions",
                ReleaseDate = new DateOnly(2003, 11, 5),
                Genre = "Sci-fi (Action)",
                Price = 5.21M,
                Rating = "R"
            },
            new Movie
            {
                Title = "Dune",
                ReleaseDate = new DateOnly(2021, 10, 22),
                Genre = "Sci-fi (Epic)",
                Price = 10.15M,
                Rating = "PG-13"
            },
            new Movie
            {
                Title = "Dune: Part Two",
                ReleaseDate = new DateOnly(2024, 3, 1),
                Genre = "Sci-fi (Epic)",
                Price = 12.09M,
                Rating = "PG-13"
            },
            new Movie
            {
                Title = "Interstellar",
                ReleaseDate = new DateOnly(2014, 11, 7),
                Genre = "Sci-fi (Adventure)",
                Price = 8.02M,
                Rating = "PG-13"
            },
            new Movie
            {
                Title = "Arrival",
                ReleaseDate = new DateOnly(2016, 11, 11),
                Genre = "Sci-fi (Drama)",
                Price = 7.43M,
                Rating = "PG-13"
            },
            new Movie
            {
                Title = "Edge of Tomorrow",
                ReleaseDate = new DateOnly(2014, 6, 6),
                Genre = "Sci-fi (Action)",
                Price = 6.88M,
                Rating = "PG-13"
            },
            new Movie
            {
                Title = "Oblivion",
                ReleaseDate = new DateOnly(2013, 4, 19),
                Genre = "Sci-fi (Adventure)",
                Price = 6.11M,
                Rating = "PG-13"
            },
            new Movie
            {
                Title = "Tron: Legacy",
                ReleaseDate = new DateOnly(2010, 12, 17),
                Genre = "Sci-fi (Action)",
                Price = 5.04M,
                Rating = "PG"
            },
            new Movie
            {
                Title = "Star Wars: A New Hope",
                ReleaseDate = new DateOnly(1977, 5, 25),
                Genre = "Sci-fi (Space Opera)",
                Price = 4.99M,
                Rating = "PG"
            },
            new Movie
            {
                Title = "Star Wars: The Empire Strikes Back",
                ReleaseDate = new DateOnly(1980, 5, 21),
                Genre = "Sci-fi (Space Opera)",
                Price = 5.67M,
                Rating = "PG"
            });

        context.SaveChanges();
    }
}
