export interface VotingCandidate {
    songId: string;
    title: string;
    album: string;
    artist: string;
    coverImageId?: string;
    voteCount: number;
    isActive: boolean;
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

export const SET_SELECTED_VOTING_CANDIDATE = "SET_SELECTED_VOTING_CANDIDATE";
interface SelectVotingCandidateAction {
    type: typeof SET_SELECTED_VOTING_CANDIDATE,
    payload: { songId: string | null }
}

export const DISABLE_VOTING = "DISABLE_VOTING";
interface DisableVotingAction {
    type: typeof DISABLE_VOTING
}

export type VotingActionTypes = SetVotingCandidatesAction | SelectVotingCandidateAction | DisableVotingAction;