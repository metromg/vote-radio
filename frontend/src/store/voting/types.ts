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
    selectedSongId: string | null;
}

export const SET_VOTING_CANDIDATES = "SET_VOTING_CANDIDATES";
interface SetVotingCandidatesAction {
    type: typeof SET_VOTING_CANDIDATES
    payload: { candidates: VotingCandidate[] }
}

export const SELECT_VOTING_CANDIDATE = "SELECT_VOTING_CANDIDATE";
interface SelectVotingCandidateAction {
    type: typeof SELECT_VOTING_CANDIDATE,
    payload: { songId: string }
}

export type VotingActionTypes = SetVotingCandidatesAction | SelectVotingCandidateAction;