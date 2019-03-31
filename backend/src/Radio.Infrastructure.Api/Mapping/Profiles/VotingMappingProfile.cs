using Radio.Core.Domain.Voting.Objects;
using Radio.Infrastructure.Api.Dtos;

namespace Radio.Infrastructure.Api.Mapping.Profiles
{
    public class VotingMappingProfile : MappingProfileBase
    {
        public VotingMappingProfile()
            : base("Voting")
        {
            MapEntitiesToDtos();
            MapDtosToEntities();
        }

        private void MapEntitiesToDtos()
        {
            CreateMap<SongWithVoteCount, VotingCandidateDto>()
                .ForMember(e => e.Title, opt => opt.MapFrom(s => s.Song.Title))
                .ForMember(e => e.Album, opt => opt.MapFrom(s => s.Song.Album))
                .ForMember(e => e.Artist, opt => opt.MapFrom(s => s.Song.Artist))
                .ForMember(e => e.CoverImageId, opt => opt.MapFrom(s => s.Song.CoverImageId))
                .ForMember(e => e.VoteCount, opt => opt.MapFrom(s => s.VoteCount));
        }

        private void MapDtosToEntities()
        {

        }
    }
}
