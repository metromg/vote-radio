import { VotingState, VotingActionTypes, SET_VOTING_CANDIDATES, SELECT_VOTING_CANDIDATE, UPDATE_VOTE_COUNT } from './types';

const initialState: VotingState = {
    candidates: [],
    selectedSongId: null
}

export function votingReducer(state = initialState, action: VotingActionTypes) {
    switch (action.type) {
        case SET_VOTING_CANDIDATES:
            return Object.assign({}, state, {
                candidates: [...action.payload.candidates],
                selectedSongId: null
            });
        case SELECT_VOTING_CANDIDATE:
            return Object.assign({}, state, {
                selectedSongId: action.payload.songId
            });
        case UPDATE_VOTE_COUNT:
            return Object.assign({}, state, {
                candidates: state.candidates.map(candidate => {
                    if (candidate.songId === action.payload.songId) {
                        return Object.assign({}, candidate, {
                            voteCount: candidate.voteCount + 1
                        });
                    }

                    return candidate;
                })
            });
        default:
            return state;
    }
}