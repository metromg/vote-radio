import React, { Component } from 'react';
import { connect } from "react-redux";

import { AppState } from './../store';
import Audio from './Audio';

interface PlayerProps {
    streamBaseUrl: string;
}

class Player extends Component<PlayerProps> {
    render() {
        const streamUrl = this.props.streamBaseUrl + "/radio.mp3";

        return (
            <Audio src={streamUrl} title="DER STREAM!" />
        );
    }
}

const mapStateToProps = (state: AppState) => ({
    streamBaseUrl: state.system.streamBaseUrl
});

export default connect(mapStateToProps)(Player);