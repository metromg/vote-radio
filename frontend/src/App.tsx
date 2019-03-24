import React from 'react';
import { connect } from "react-redux";
import { AppState } from './store';

import { SystemState } from './store/system/types';
import './App.css';

interface AppProps {
    system: SystemState;
}

const App = (props: AppProps) => (
    <p>
        Hello World<br />
        {props.system.apiBaseUrl}<br />
        {props.system.streamBaseUrl}
    </p>
);

const mapStateToProps = (state: AppState) => ({
    system: state.system
});

export default connect(mapStateToProps)(App);