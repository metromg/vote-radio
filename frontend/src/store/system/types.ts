export interface SystemState {
    apiBaseUrl: string
    streamBaseUrl: string
}

export const UPDATE_BASE_URLS = "UPDATE_BASE_URLS";
interface UpdateBaseUrlsAction {
    type: typeof UPDATE_BASE_URLS
    payload: SystemState
}

export type SystemActionTypes = UpdateBaseUrlsAction;