using AutoMapper;
using EventConnect.Dtos.Posts;
using EventConnect.Entities.Post;

namespace EventConnect;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Post,AddPostDto>().ReverseMap();
        CreateMap<Post,GetPostDto>().ReverseMap();
        CreateMap<Post,UpdatePostDto>().ReverseMap();
 
    }
}