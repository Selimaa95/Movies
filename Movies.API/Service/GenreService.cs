using Microsoft.EntityFrameworkCore;

namespace Movies.API.Service
{
    public class GenreService : IGenreService
    {
        #region Prop
        private readonly ApplicationDbContext db;

        #endregion

        #region Ctor

        public GenreService(ApplicationDbContext db)
        {
            this.db = db;
        }

        #endregion

        #region Methods

        public async Task<IEnumerable<Genre>> GetAll()
        {
             return await db.Genres.OrderBy(g => g.Name).ToListAsync();   
        }
        public async Task<Genre> GetById(byte id)
        {
            return await db.Genres.FindAsync(id);        
        }
        public async Task<Genre> Add(Genre model)
        {           
            await db.AddAsync(model);
            await db.SaveChangesAsync();
            return model;
        }
        public Genre Update(Genre model)
        {
            db.Update(model);
            db.SaveChanges();
            return model;
        }
        public Genre Delete(Genre model)
        {
            db.Genres.Remove(model);
            db.SaveChanges();
            return model;
        }

        public Task<bool> isValidGenre(byte id)
        {
            return db.Genres.AnyAsync(g => g.Id == id);
        }

        #endregion
    }
}
