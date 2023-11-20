using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.OpenApi.Validations;
using Movies.API.Models;
using Movies.API.Service;

namespace Movies.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {

        #region Prop
        private readonly IMovieService service;
        private readonly IGenreService genreService;
        private readonly IMapper mapper;
        private new List<string> allowedExtentions = new() { ".jpg",".png"};
        private long maxAllowedSize = 1048576;

        #endregion

        #region Ctor
        public MoviesController(IMovieService service, IGenreService genreService, IMapper mapper)
        {
            this.service = service;
            this.genreService = genreService;
            this.mapper = mapper;
        }
        #endregion

        #region EndPoints
         
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var movies = await service.GetAll();
            var result = mapper.Map<IEnumerable<MovieDetailsDto>>(movies);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var movie = await service.GetById(id);              
            if (movie is null)
                return NotFound("Movie Not Exists");

            var result = mapper.Map<MovieDetailsDto>(movie);           
            return Ok(result);
        }

        [HttpGet("GetMoviesByGenreId")]
        public async Task<IActionResult> GetMoviesByGenreId(byte genreId)
        {
            var movies = await service.GetByGenreId(genreId);
            var result = mapper.Map<IEnumerable<MovieDetailsDto>>(movies);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] MovieDto dto)
        {
            if (dto.Poster is null)
                return BadRequest("Poster Field is Required");

            if (!allowedExtentions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("Only .png and .jpg images are allowed!");

            if (dto.Poster.Length > maxAllowedSize)
                return BadRequest("Max allowed Size For Poster is 1MB");

            var isValidGenre = await genreService.isValidGenre(dto.GenreId);
            if (!isValidGenre)
                return BadRequest("Invalid Genre Id");

            using var stream = new MemoryStream();
            await dto.Poster.CopyToAsync(stream);

            var movie = mapper.Map<Movie>(dto);
            movie.Poster = stream.ToArray();

            await service.Add(movie);
            return Ok(movie);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromForm] MovieDto dto)
        {
            var data = await service.GetById(id);
            if (data is null)
                return NotFound($"No Movie was found with Id {id}");

            var isValidGenre = await genreService.isValidGenre(dto.GenreId);
            if (!isValidGenre) 
                return BadRequest("Invalid Genre Id");

            var movie = mapper.Map<Movie>(dto);

            if(dto.Poster is not null)
            {
                if (!allowedExtentions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                    return BadRequest("Only .png and .jpg images are allowed!");
                if (dto.Poster.Length > maxAllowedSize)
                    return BadRequest("Max allowed Size For Poster is 1MB");

                using var stream = new MemoryStream();
                await dto.Poster.CopyToAsync(stream);
                movie.Poster = stream.ToArray();
            }
           

            service.Update(movie);
            return Ok(movie);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var movie = await service.GetById(id);
            if (movie is null)
                return NotFound($"No movie was found with id => {id}");

            service.Delete(movie);
            return Ok(movie);
        }
        #endregion
    }
}   
