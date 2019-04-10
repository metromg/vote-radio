import { AnyAction } from 'redux';
import { ThunkAction } from 'redux-thunk';

import { AppState } from '../index';
import { VotingCandidate, SET_VOTING_CANDIDATES, SET_SELECTED_VOTING_CANDIDATE, DISABLE_VOTING, VotingActionTypes } from './types';
import { setErrorMessage } from '../error/actions';
import { get, post } from '../api';

export function loadVotingCandidates(): ThunkAction<void, AppState, null, AnyAction> {
    return async dispatch => {
        try {
            const response = await get("/api/voting/getVotingCandidatesAsync");
            const json = await response.json();
    
            dispatch(setVotingCandidates(json));
        }
        catch (e) {
            dispatch(setErrorMessage("errorConnection"));
        }
    };
}

export function setVotingCandidates(candidates: VotingCandidate[]): VotingActionTypes {
    return {
        type: SET_VOTING_CANDIDATES,
        payload: { candidates }
    };
}

export function selectVotingCandidate(songId: string): ThunkAction<void, AppState, null, AnyAction> {
    return async dispatch => {
        if (!navigator.cookieEnabled) {
            dispatch(setErrorMessage("errorVoting"));
            return;
        }

        try {
            await post("/api/voting/voteAsync?songId=" + encodeURIComponent(songId));
            dispatch(setSelectedVotingCandidate(songId));
        }
        catch (e) {
            dispatch(setErrorMessage("errorVoting"));
        }
    };
}

export function setSelectedVotingCandidate(songId: string | null): VotingActionTypes {
    return {
        type: SET_SELECTED_VOTING_CANDIDATE,
        payload: { songId }
    };
}

export function disableVoting(): VotingActionTypes {
    return {
        type: DISABLE_VOTING
    };
}