using AutoMapper;
using Radio.Core.Domain;
using Radio.Infrastructure.Api.External.Dtos;

namespace Radio.Infrastructure.Api.External.Mapping
{
    public abstract class MappingProfileBase : Profile
    {
        protected MappingProfileBase(string profileName)
            : base(profileName)
        {
        }

        protected IMappingExpression<TEntity, TDto> CreateEntityToDtoMap<TEntity, TDto>()
            where TEntity : EntityBase
            where TDto : EntityBaseDto
        {
            return CreateMap<TEntity, TDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
        }

        protected IMappingExpression<TDto, TEntity> CreateDtoToEntityMap<TDto, TEntity>()
            where TDto : EntityBaseDto
            where TEntity : EntityBase
        {
            return CreateMap<TDto, TEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
