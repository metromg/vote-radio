using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Radio.Core.Domain.Voting;
using Radio.Infrastructure.Api.Dtos;

namespace Radio.Infrastructure.Api.External.Controllers
{
    [Route("api/[controller]/[action]")]
    public class VotingController : Controller
    {
        private readonly IVotingCandidateRepository _votingCandidateRepository;
        private readonly IMapper _mapper;

        public VotingController(IVotingCandidateRepository votingCandidateRepository, IMapper mapper)
        {
            _votingCandidateRepository = votingCandidateRepository;
            _mapper = mapper;
        }

        public async Task<VotingCandidateDto[]> GetVotingCandidatesAsync()
        {
            var candidates = await _votingCandidateRepository.GetWithVoteCount();

            return candidates.Select(_mapper.Map<VotingCandidateDto>).ToArray();
        }
    }
}
