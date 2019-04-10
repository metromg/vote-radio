import { VotingState, VotingActionTypes, SET_VOTING_CANDIDATES, SET_SELECTED_VOTING_CANDIDATE, DISABLE_VOTING } from './types';

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
        case SET_SELECTED_VOTING_CANDIDATE:
            return Object.assign({}, state, {
                selectedSongId: action.payload.songId
            });
        case DISABLE_VOTING:
            return Object.assign({}, state, {
                candidates: [...state.candidates.map(c => ({ ...c, isActive: false }))]
            });
        default:
            return state;
    }
}