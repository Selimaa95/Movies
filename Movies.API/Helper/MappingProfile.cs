using AutoMapper;

namespace Movies.API.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Movie, MovieDetailsDto>();
            CreateMap<MovieDto, Movie>()
                .ForMember(src => src.Poster, opts => opts.Ignore());
            
        }
    }
}
