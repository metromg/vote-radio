import { PlaybackState, PlaybackActionTypes, SET_CURRENT_SONG, PLAY, STOP } from './types';

const initialState: PlaybackState = {
    currentSong: null,
    playing: false
}

export function playbackReducer(state = initialState, action: PlaybackActionTypes) {
    switch (action.type) {
        case SET_CURRENT_SONG:
            return Object.assign({}, state, {
                currentSong: {...action.payload.currentSong}
            });
        case PLAY:
            return Object.assign({}, state, {
                playing: true
            });
        case STOP:
            return Object.assign({}, state, {
                playing: false
            });
        default:
            return state;
    }
}