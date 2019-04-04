import { VotingState, VotingActionTypes, SET_VOTING_CANDIDATES, SELECT_VOTING_CANDIDATE } from './types';

const initialState: VotingState = {
    candidates: [],
    selectedSongId: null
}

export function votingReducer(state = initialState, action: VotingActionTypes) {
    switch (action.type) {
        case SET_VOTING_CANDIDATES:
            return Object.assign({}, state, {
                candidates: [...action.payload.candidates]
            });
        case SELECT_VOTING_CANDIDATE:
            return Object.assign({}, state, {
                selectedSongId: action.payload.songId
            });
        default:
            return state;
    }
}