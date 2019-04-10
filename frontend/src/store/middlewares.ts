import { Middleware, MiddlewareAPI } from 'redux';
import { HubConnectionBuilder, LogLevel, HubConnection } from '@aspnet/signalr';

import { CurrentSong } from './playback/types';
import { VotingCandidate } from './voting/types';
import { loadCurrentSong, setCurrentSong } from './playback/actions';
import { loadVotingCandidates, setVotingCandidates, setSelectedVotingCandidate, disableVoting } from './voting/actions';
import { setErrorMessage } from './error/actions';

export const signalRMiddleware: (url: string) => Middleware = url => storeAPI => {
    const connection = new HubConnectionBuilder()
        .withUrl(url)
        .configureLogging(LogLevel.Information)
        .build();
        
    connection.on("NextSong", (currentSong: CurrentSong, candidates: VotingCandidate[]) => {
        storeAPI.dispatch(setCurrentSong(currentSong));
        storeAPI.dispatch(setVotingCandidates(candidates));
        storeAPI.dispatch(setSelectedVotingCandidate(null));
    });

    connection.on("Vote", (candidates: VotingCandidate[]) => {
        storeAPI.dispatch(setVotingCandidates(candidates));
    });

    connection.on("DisableVoting", () => {
        storeAPI.dispatch(disableVoting());
    });

    connection.onclose(() => {
        // if the connection is closed, restore the connection and refresh everything (e.g. when suspending on mobile)
        startConnection(connection, () => dispatchConnectionError(storeAPI));
        storeAPI.dispatch<any>(loadCurrentSong());
        storeAPI.dispatch<any>(loadVotingCandidates());
    });
    
    startConnection(connection, () => dispatchConnectionError(storeAPI));

    return next => action => {
        return next(action);
    };
}

function startConnection(connection: HubConnection, onError?: (e: any) => void) {
    connection.start()
        .catch(e => {
            console.log(e);

            if (onError != null) {
                onError(e);
            }
        });
}

function dispatchConnectionError(storeAPI: MiddlewareAPI) {
    storeAPI.dispatch(setErrorMessage("errorConnection"));
}