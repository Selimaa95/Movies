using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.API.Models;
using Movies.API.Service;

namespace Movies.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        #region Prop
        private readonly IGenreService service;


        #endregion

        #region Ctor

        public GenresController(IGenreService service)
        {
            this.service = service;
        }
        #endregion

        #region EndPoints

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var genres = await service.GetAll();
            return Ok(genres);
        } 

        [HttpPost]
        public async Task<IActionResult> CreateAsync(GenreDto dto)
        {
            var genre = new Genre()  { Name = dto.Name };          
            await service.Add(genre);

            return Ok(genre);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(byte id,[FromBody] GenreDto dto)
        {
            var genre = await service.GetById(id);

            if (genre is null)
                return NotFound($"No genre was found With Id: {id}");

             genre.Name = dto.Name;
             service.Update(genre);
            return Ok(genre);    
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(byte id)
        {
            var genre = await service.GetById(id);
            if (genre is null)
                return NotFound($"No genre was found With Id: {id}");

            service.Delete(genre);
            return Ok(genre);
        } 
        #endregion
    }
}
    