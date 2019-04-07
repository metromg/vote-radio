import { ThunkAction } from 'redux-thunk';

import { AppState } from '../index';
import { CurrentSong, SET_CURRENT_SONG, PLAY, STOP, PlaybackActionTypes } from './types';
import { get } from '../api';

// TODO: Error handling
export function loadCurrentSong(): ThunkAction<void, AppState, null, PlaybackActionTypes> {
    return async dispatch => {
        const response = await get("/api/playback/getCurrentSongAsync");
        const json = await response.json();

        dispatch(setCurrentSong(json));
    };
}

export function setCurrentSong(currentSong: CurrentSong): PlaybackActionTypes {
    return {
        type: SET_CURRENT_SONG,
        payload: { currentSong }
    };
}

export function play(): PlaybackActionTypes {
    return {
        type: PLAY
    };
}

export function stop(): PlaybackActionTypes {
    return {
        type: STOP
    };
}