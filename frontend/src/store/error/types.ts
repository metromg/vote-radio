export interface ErrorState {
    errorMessageKey: string | null
}

export const SET_ERROR_MESSAGE = "SET_ERROR_MESSAGE";
interface SetErrorMessageAction {
    type: typeof SET_ERROR_MESSAGE,
    payload: { errorMessageKey: string }
}

export const DISMISS_ERROR = "DISMISS_ERROR";
interface DismissErrorAction {
    type: typeof DISMISS_ERROR
}

export type ErrorActionTypes = SetErrorMessageAction | DismissErrorAction;