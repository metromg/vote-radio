import { ErrorState, ErrorActionTypes, SET_ERROR_MESSAGE, DISMISS_ERROR } from './types';

const initialState: ErrorState = {
    errorMessageKey: null
}

export function errorReducer(state = initialState, action: ErrorActionTypes) {
    switch (action.type) {
        case SET_ERROR_MESSAGE:
            return {...state, errorMessageKey: action.payload.errorMessageKey };
        case DISMISS_ERROR:
            return {...state, errorMessageKey: null };
        default:
            return state;
    }
}