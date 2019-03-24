import { SystemState, SystemActionTypes, UPDATE_BASE_URLS } from './types';

const initialState: SystemState = {
    apiBaseUrl: '',
    streamBaseUrl: ''
}

export function systemReducer(state = initialState, action: SystemActionTypes): SystemState {
    switch (action.type) {
        case UPDATE_BASE_URLS:
            return {
                ...state,
                ...action.payload
            };
        default:
            return state;
    }
}