import { Middleware, MiddlewareAPI } from 'redux';
import { HubConnectionBuilder, LogLevel, HubConnection } from '@aspnet/signalr';

import { VotingCandidate } from './voting/types';
import { loadVotingCandidates, setVotingCandidates } from './voting/actions';

export const signalRMiddleware: (url: string) => Middleware = url => storeAPI => {
    const connection = new HubConnectionBuilder()
        .withUrl(url)
        .configureLogging(LogLevel.Information)
        .build();

    // here we can handle the events on the hub and dispatch redux actions
    connection.on("Vote", (candidates: VotingCandidate[]) => {
        storeAPI.dispatch(setVotingCandidates(candidates));
    });

    connection.onclose(() => {
        // if the connection is closed, restore the connection and refresh everything (e.g. when suspending on mobile)
        startConnection(connection, () => dispatchConnectionError(storeAPI));
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
    // dispatch a redux action
    console.log("Error action");
}