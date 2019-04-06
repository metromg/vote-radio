using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Radio.Core;
using Radio.Core.Domain.Voting;
using Radio.Core.Services;
using Radio.Core.Services.Voting;
using Radio.Infrastructure.Api.External.Dtos;
using Radio.Infrastructure.Api.Services;

namespace Radio.Infrastructure.Api.External.Controllers
{
    [Route("api/[controller]/[action]")]
    public class VotingController : Controller
    {
        private readonly IVotingCandidateRepository _votingCandidateRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPrimitiveUserIdentificationService _primitiveUserIdentificationService;
        private readonly IVoteService _voteService;
        private readonly IMessageQueueService _messageQueueService;
        private readonly IMapper _mapper;

        public VotingController(IVotingCandidateRepository votingCandidateRepository, IUnitOfWork unitOfWork, IPrimitiveUserIdentificationService primitiveUserIdentificationService, IVoteService voteService, IMessageQueueService messageQueueService, IMapper mapper)
        {
            _votingCandidateRepository = votingCandidateRepository;
            _unitOfWork = unitOfWork;
            _primitiveUserIdentificationService = primitiveUserIdentificationService;
            _voteService = voteService;
            _messageQueueService = messageQueueService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<VotingCandidateDto[]> GetVotingCandidatesAsync()
        {
            var votingCandidates = await _votingCandidateRepository.GetWithVoteCountAsync();

            return votingCandidates.Select(_mapper.Map<VotingCandidateDto>).ToArray();
        }

        [HttpPost]
        public async Task VoteAsync(Guid songId)
        {
            var votingCandidate = await _votingCandidateRepository.GetBySongAsync(songId);
            var userIdentifier = _primitiveUserIdentificationService.GetOrCreateUserId(HttpContext);

            await _voteService.UpdateOrCreateAsync(votingCandidate, userIdentifier);

            await _unitOfWork.CommitAsync();

            _messageQueueService.Send(Message.VotingMessage);
        }
    }
}
