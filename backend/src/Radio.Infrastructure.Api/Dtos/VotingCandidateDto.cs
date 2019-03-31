using System;

namespace Radio.Infrastructure.Api.Dtos
{
    public class VotingCandidateDto
    {
        public string Title { get; set; }

        public string Album { get; set; }

        public string Artist { get; set; }

        public Guid? CoverImageId { get; set; }

        public int VoteCount { get; set; }
    }
}
