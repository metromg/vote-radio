import React from 'react';
import ReactDOM from 'react-dom';
import { Provider } from "react-redux";
import { LocalizeProvider } from 'react-localize-redux';

import configureStore from "./store";
import { updateBaseUrls } from './store/system/actions';
import App from './App';
import './index.css';

const store = configureStore();

store.dispatch(updateBaseUrls({ 
    apiBaseUrl: (window as any).systemConfiguration.apiBaseUrl, 
    streamBaseUrl: (window as any).systemConfiguration.streamBaseUrl
}));

const Root = () => (
    <Provider store={store}>
        <LocalizeProvider store={store}>
            <App />
        </LocalizeProvider>
    </Provider>
);

ReactDOM.render(<Root />, document.getElementById('root'));