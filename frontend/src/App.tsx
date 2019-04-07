import React, { Component } from 'react';
import { renderToStaticMarkup } from "react-dom/server";
import { withLocalize, LocalizeContextProps } from "react-localize-redux";

import translations from './translations.json';
import Playback from './components/playback/Playback';
import Voting from './components/Voting';
import './App.css';

class App extends Component<LocalizeContextProps> {
    constructor(props: LocalizeContextProps) {
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
            <React.Fragment>
                <header className="app-header">
                    <div className="app-header-inner container">
                        <div className="app-header-logo">Vote Radio</div>
                    </div>
                </header>
                <div className="app-content container">
                    <Playback />
                    <Voting />
                </div>
            </React.Fragment>
        );
    }
}

export default withLocalize(App);