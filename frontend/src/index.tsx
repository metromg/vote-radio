import 'react-app-polyfill/ie11';

import React from 'react';
import ReactDOM from 'react-dom';
import { Provider } from "react-redux";
import { LocalizeProvider } from 'react-localize-redux';

import configureStore from "./store";
import App from './App';
import './index.css';

const store = configureStore();

const Root = () => (
    <Provider store={store}>
        <LocalizeProvider store={store}>
            <App />
        </LocalizeProvider>
    </Provider>
);

ReactDOM.render(<Root />, document.getElementById('root'));