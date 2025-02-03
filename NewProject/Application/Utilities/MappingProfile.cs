using AutoMapper;

namespace Application.Utilities;

public class MappingConfig<TSource, TDestination> : Profile
{
    public MappingConfig()
    {
        CreateMap<TSource, TDestination>();
    }
}

public class MappingProfile<TSource, TDestination> where TSource : class
{
    private readonly IMapper _mapper;

    public MappingProfile()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingConfig<TSource, TDestination>());
        });

        _mapper = config.CreateMapper();
    }

    public TDestination Map(TSource source)
    {
        return _mapper.Map<TSource, TDestination>(source);
    }
}