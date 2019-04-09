import { SET_ERROR_MESSAGE, DISMISS_ERROR, ErrorActionTypes } from './types';

export function setErrorMessage(errorMessageKey: string): ErrorActionTypes {
    return {
        type: SET_ERROR_MESSAGE,
        payload: { errorMessageKey }
    };
}

export function dismissError(): ErrorActionTypes {
    return {
        type: DISMISS_ERROR
    };
}