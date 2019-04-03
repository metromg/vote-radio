import { ThunkAction } from 'redux-thunk';

import { AppState } from '../index';
import { VotingCandidate, SET_VOTING_CANDIDATES, SELECT_VOTING_CANDIDATE, VotingActionTypes } from './types';
import { apiBaseUrl } from '../../config';

export function loadVotingCandidates(): ThunkAction<void, AppState, null, VotingActionTypes> {
    return async dispatch => {
        const response = await fetch(apiBaseUrl + "/api/voting/getVotingCandidatesAsync");
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

export function selectVotingCandidate(songId: string): ThunkAction<void, AppState, null, VotingActionTypes> {
    return async dispatch => {
        await fetch(apiBaseUrl + "/api/voting/voteAsync?songId=" + encodeURIComponent(songId), { method: "POST", credentials: "include" });

        dispatch({
            type: SELECT_VOTING_CANDIDATE,
            payload: { songId }
        });
    };
}