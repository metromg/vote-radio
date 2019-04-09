import { AnyAction } from 'redux';
import { ThunkAction } from 'redux-thunk';

import { AppState } from '../index';
import { CurrentSong, SET_CURRENT_SONG, PLAY, STOP, PlaybackActionTypes } from './types';
import { setErrorMessage } from '../error/actions';
import { get } from '../api';

export function loadCurrentSong(): ThunkAction<void, AppState, null, AnyAction> {
    return async dispatch => {
        try {
            const response = await get("/api/playback/getCurrentSongAsync");
            const json = await response.json();

            dispatch(setCurrentSong(json));
        }
        catch (e) {
            dispatch(setErrorMessage("errorConnection"));
        }
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