import React, { Component } from 'react';
import { connect } from 'react-redux';
import { Dispatch } from 'redux';
import { renderToStaticMarkup } from "react-dom/server";
import { withLocalize, LocalizeContextProps } from "react-localize-redux";

import { AppState } from './store/index';
import { loadCurrentSong } from './store/playback/actions';
import { loadVotingCandidates } from './store/voting/actions';
import translations from './translations.json';
import Header from './components/Header';
import GlobalLoadingIndicator from './components/GlobalLoadingIndicator';
import GlobalError from './components/GlobalError';
import Playback from './components/playback/Playback';
import Voting from './components/Voting';
import './App.css';

interface AppProps extends LocalizeContextProps {
    loading: boolean;

    loadCurrentSong: () => void;
    loadVotingCandidates: () => void;
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

    componentDidMount() {
        this.props.loadCurrentSong();
        this.props.loadVotingCandidates();
    }

    render() {
        return (
            <React.Fragment>
                <header className="app-header">
                    <Header />
                </header>
                <div className="app-content container">
                    <GlobalLoadingIndicator loading={this.props.loading}>
                        <Playback />
                        <Voting />
                    </GlobalLoadingIndicator>
                    <GlobalError />
                </div>
            </React.Fragment>
        );
    }
}

const mapStateToProps = (state: AppState) => ({
    loading: state.playback.currentSong == null || state.voting.candidates.length == 0
});

const mapDispatchToProps = (dispatch: Dispatch) => ({
    loadCurrentSong: () => dispatch<any>(loadCurrentSong()),
    loadVotingCandidates: () => dispatch<any>(loadVotingCandidates())
});

export default withLocalize(connect(mapStateToProps, mapDispatchToProps)(App));