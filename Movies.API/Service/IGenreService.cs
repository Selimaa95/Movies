namespace Movies.API.Service
{
    public interface IGenreService
    {
        Task<IEnumerable<Genre>> GetAll();
        Task<Genre> GetById(byte id);
        Task<Genre> Add(Genre model);
        Genre Update(Genre model);
        Genre Delete(Genre model);
        Task<bool> isValidGenre(byte id); 
    }
}
