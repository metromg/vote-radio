using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Radio.Core;
using Radio.Core.Domain.Voting;
using Radio.Core.Services;
using Radio.Infrastructure.Api.External.Dtos;
using Radio.Infrastructure.Api.Services;

namespace Radio.Infrastructure.Api.External.Controllers
{
    [Route("api/[controller]/[action]")]
    public class VotingController : Controller
    {
        private readonly IVotingCandidateRepository _votingCandidateRepository;
        private readonly IVoteRepository _voteRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPrimitiveUserIdentificationService _primitiveUserIdentificationService;
        private readonly IMessageQueueService _messageQueueService;
        private readonly IMapper _mapper;

        public VotingController(IVotingCandidateRepository votingCandidateRepository, IVoteRepository voteRepository, IUnitOfWork unitOfWork, IPrimitiveUserIdentificationService primitiveUserIdentificationService, IMessageQueueService messageQueueService, IMapper mapper)
        {
            _votingCandidateRepository = votingCandidateRepository;
            _voteRepository = voteRepository;
            _unitOfWork = unitOfWork;
            _primitiveUserIdentificationService = primitiveUserIdentificationService;
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
            // TODO: Move code to domain model
            var userIdentifier = _primitiveUserIdentificationService.GetOrCreateUserId(HttpContext);

            var vote = await _voteRepository.GetByUserIdentifierOrDefaultAsync(userIdentifier);
            if (vote == null)
            {
                vote = _voteRepository.Create();
                vote.UserIdentifier = userIdentifier;

                _voteRepository.Add(vote);
            }

            var votingCandidate = await _votingCandidateRepository.GetBySongAsync(songId);
            vote.VotingCandidateId = votingCandidate.Id;
            vote.VotingCandidate = votingCandidate;

            await _unitOfWork.CommitAsync();

            _messageQueueService.Send(Message.VotingMessage);
        }
    }
}
