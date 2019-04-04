import React, { Component } from 'react';
import { renderToStaticMarkup } from "react-dom/server";
import { withLocalize, LocalizeContextProps } from "react-localize-redux";

import translations from './translations.json';
import Player from './components/Player';
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
                <Player />
                <Voting />
            </React.Fragment>
        );
    }
}

export default withLocalize(App);