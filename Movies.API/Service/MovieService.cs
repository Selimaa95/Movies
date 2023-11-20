using Microsoft.EntityFrameworkCore;

namespace Movies.API.Service
{
    public class MovieService : IMovieService
    {
        #region Prop
        private readonly ApplicationDbContext db;

        #endregion

        #region Ctor
        public MovieService(ApplicationDbContext db)
        {
            this.db = db;
        }
        #endregion

        #region Methods

        public async Task<IEnumerable<Movie>> GetAll()
        {
            return await db.Movies
                .OrderByDescending(x => x.Rate)
                .Include(m => m.Genre)
                .ToListAsync();
        }

        public async Task<Movie> GetById(int id)
        {
            return await db.Movies.Include(m => m.Genre).SingleOrDefaultAsync(m => m.Id == id);
        }
        public async Task<IEnumerable<Movie>> GetByGenreId(byte genreId)
        {
            return await db.Movies
                .Where(m => m.GenreId == genreId)
                .OrderByDescending(m => m.Rate)
                .Include(m => m.Genre)
                .ToListAsync();
        }

        public async Task<Movie> Add(Movie movie)
        {
            await db.Movies.AddAsync(movie);
            await db.SaveChangesAsync();
            return movie;
        }

        public Movie Update(Movie movie)
        {
            db.Movies.Update(movie);
            db.SaveChanges();
            return movie;
        }

        public Movie Delete(Movie movie)
        {
            db.Movies.Remove(movie);
            db.SaveChanges();
            return movie;
        }


        #endregion
    }
}
