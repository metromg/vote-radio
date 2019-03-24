import { combineReducers, createStore, applyMiddleware } from 'redux';
import thunkMiddleware from 'redux-thunk';

import { systemReducer } from './system/reducers';
import { localizeReducer } from 'react-localize-redux';

const rootReducer = combineReducers({
    system: systemReducer,
    localize: localizeReducer
});

export type AppState = ReturnType<typeof rootReducer>;

export default function configureStore() {
    const middlewares = [thunkMiddleware];
    const middleWareEnhancer = applyMiddleware(...middlewares);

    const store = createStore(
        rootReducer,
        middleWareEnhancer
    );

    return store;
}