import { SystemState, UPDATE_BASE_URLS, SystemActionTypes } from './types';

export function updateBaseUrls(newBaseUrls: SystemState): SystemActionTypes {
    return {
        type: UPDATE_BASE_URLS,
        payload: newBaseUrls
    };
}