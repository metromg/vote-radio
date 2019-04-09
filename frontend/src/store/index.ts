import { combineReducers, createStore, applyMiddleware } from 'redux';
import thunkMiddleware from 'redux-thunk';
import { localizeReducer } from 'react-localize-redux';

import { errorReducer } from './error/reducers';
import { playbackReducer } from './playback/reducers';
import { votingReducer } from './voting/reducers';
import { signalRMiddleware } from './middlewares';
import { apiBaseUrl } from '../config';

const rootReducer = combineReducers({
    localize: localizeReducer,
    error: errorReducer,
    playback: playbackReducer,
    voting: votingReducer
});

export type AppState = ReturnType<typeof rootReducer>;

export default function configureStore() {
    const middlewares = [thunkMiddleware, signalRMiddleware(apiBaseUrl + "/radioHub")];
    const middleWareEnhancer = applyMiddleware(...middlewares);

    const store = createStore(
        rootReducer,
        middleWareEnhancer
    );

    return store;
}