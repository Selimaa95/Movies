using Microsoft.AspNetCore.Mvc;
using Movies.API.Models;

namespace Movies.API.Service
{
    public interface IMovieService
    {
        Task<IEnumerable<Movie>> GetAll();
        Task<Movie> GetById(int id);
        Task<IEnumerable<Movie>> GetByGenreId(byte genreId);
        Task<Movie> Add(Movie movie);
        Movie Update(Movie movie);
        Movie Delete(Movie movie);
    }
}
