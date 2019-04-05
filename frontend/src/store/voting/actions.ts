import { ThunkAction } from 'redux-thunk';

import { AppState } from '../index';
import { VotingCandidate, SET_VOTING_CANDIDATES, SET_SELECTED_VOTING_CANDIDATE, VotingActionTypes } from './types';
import { get, post } from '../api';

// TODO: Error handling
export function loadVotingCandidates(): ThunkAction<void, AppState, null, VotingActionTypes> {
    return async dispatch => {
        const response = await get("/api/voting/getVotingCandidatesAsync");
        const json = await response.json();

        dispatch(setVotingCandidates(json));
    };
}

export function setVotingCandidates(candidates: VotingCandidate[]): VotingActionTypes {
    return {
        type: SET_VOTING_CANDIDATES,
        payload: { candidates }
    };
}

// TODO: Error handling
export function selectVotingCandidate(songId: string): ThunkAction<void, AppState, null, VotingActionTypes> {
    return async dispatch => {
        if (!navigator.cookieEnabled) {
            // TODO: dispatch error message, that cookies must be enabled for voting
            return;
        }

        await post("/api/voting/voteAsync?songId=" + encodeURIComponent(songId));

        dispatch(setSelectedVotingCandidate(songId));
    };
}

export function setSelectedVotingCandidate(songId: string | null): VotingActionTypes {
    return {
        type: SET_SELECTED_VOTING_CANDIDATE,
        payload: { songId }
    };
}