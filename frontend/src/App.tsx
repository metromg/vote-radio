import React, { Component } from 'react';
import { renderToStaticMarkup } from "react-dom/server";
import { connect } from "react-redux";
import { withLocalize, LocalizeContextProps, Translate } from "react-localize-redux";

import { AppState } from './store';
import { SystemState } from './store/system/types';
import translations from './translations.json';
import './App.css';

interface AppProps extends LocalizeContextProps {
    system: SystemState;
}

class App extends Component<AppProps> {
    constructor(props: AppProps) {
        super(props);

        this.props.initialize({
            languages: [
                { name: "English", code: "en" }
            ],
            translation: translations,
            options: { renderToStaticMarkup }
        });
    }

    render() {
        return (
            <p>
                <Translate id="helloworld" /><br />
                {this.props.system.apiBaseUrl}<br />
                {this.props.system.streamBaseUrl}
            </p>
        );
    }
}

const mapStateToProps = (state: AppState) => ({
    system: state.system
});

export default withLocalize(connect(mapStateToProps)(App));