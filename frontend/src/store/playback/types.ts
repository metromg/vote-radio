export interface CurrentSong {
    songId: string;
    title: string;
    album: string;
    artist: string;
    coverImageId?: string;
    voteCount: number;
    durationInSeconds: number;
    endsAtTime: Date;
}

export interface PlaybackState {
    currentSong: CurrentSong | null,
    playing: boolean
}

export const SET_CURRENT_SONG = "SET_CURRENT_SONG";
interface SetCurrentSongAction {
    type: typeof SET_CURRENT_SONG,
    payload: { currentSong: CurrentSong }
}

export const PLAY = "PLAY";
interface PlayAction {
    type: typeof PLAY
}

export const STOP = "STOP";
interface StopAction {
    type : typeof STOP
}

export type PlaybackActionTypes = SetCurrentSongAction | PlayAction | StopAction;