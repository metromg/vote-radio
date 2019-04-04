import React, { Component } from 'react';

import AudioStream from './AudioStream';
import { streamBaseUrl } from '../config';

interface PlayerProps {
}

interface PlayerState {
    playing: boolean;
}

class Player extends Component<PlayerProps, PlayerState> {
    constructor(props: PlayerProps) {
        super(props);
        this.state = { playing: false };
    }

    render() {
        const streamUrl = streamBaseUrl + "/radio.mp3";

        return (
            <React.Fragment>
                <AudioStream 
                    src={streamUrl} 
                    title="DER STREAM!" 
                    playing={this.state.playing}
                    onLoadingAbort={() => this.stop()}
                    onLoadingError={() => this.stop()}
                    onStreamEnded={() => this.stop()}
                />
                <button onClick={() => this.play()}>Play</button>
                <button onClick={() => this.stop()}>Stop</button>
            </React.Fragment>
        );
    }

    private play() {
        this.setState({ playing: true });
    }

    private stop() {
        this.setState({ playing: false });
    }
}

export default Player;