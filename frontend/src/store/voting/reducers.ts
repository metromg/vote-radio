import { VotingState, VotingActionTypes, UPDATE_VOTE_COUNT, SET_VOTING_CANDIDATES } from './types';

const initialState: VotingState = {
    candidates: []
}

export function votingReducer(state = initialState, action: VotingActionTypes) {
    switch (action.type) {
        case SET_VOTING_CANDIDATES:
            return Object.assign({}, state, {
                candidates: [...action.payload.candidates]
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