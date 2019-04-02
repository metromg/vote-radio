export interface VotingCandidate {
    songId: string;
    title: string;
    album: string;
    artist: string;
    coverImageId?: string;
    voteCount: number;
}

export interface VotingState {
    candidates: VotingCandidate[];
}

export const SET_VOTING_CANDIDATES = "SET_VOTING_CANDIDATES";
interface SetVotingCandidatesAction {
    type: typeof SET_VOTING_CANDIDATES
    payload: { candidates: VotingCandidate[] }
}

export const UPDATE_VOTE_COUNT = "UPDATE_VOTE_COUNT";
interface UpdateVoteCountAction {
    type: typeof UPDATE_VOTE_COUNT
    payload: { songId: string }
}

export type VotingActionTypes = SetVotingCandidatesAction | UpdateVoteCountAction;