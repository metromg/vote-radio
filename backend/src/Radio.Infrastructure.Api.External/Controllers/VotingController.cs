using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Radio.Core;
using Radio.Core.Domain.Voting;
using Radio.Core.Services;
using Radio.Core.Services.Messaging;
using Radio.Infrastructure.Api.External.Dtos;

namespace Radio.Infrastructure.Api.External.Controllers
{
    [Route("api/[controller]/[action]")]
    public class VotingController : Controller
    {
        private readonly IVotingCandidateRepository _votingCandidateRepository;
        private readonly IVoteRepository _voteRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageQueueService _messageQueueService;
        private readonly IMapper _mapper;

        public VotingController(IVotingCandidateRepository votingCandidateRepository, IVoteRepository voteRepository, IUnitOfWork unitOfWork, IMessageQueueService messageQueueService, IMapper mapper)
        {
            _votingCandidateRepository = votingCandidateRepository;
            _voteRepository = voteRepository;
            _unitOfWork = unitOfWork;
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

            var newVote = _voteRepository.Create();
            newVote.VotingCandidateId = votingCandidate.Id;
            newVote.VotingCandidate = votingCandidate;

            _voteRepository.Add(newVote);

            await _unitOfWork.CommitAsync();

            _messageQueueService.Send(new VoteMessage
            {
                SongId = songId
            });
        }
    }
}
