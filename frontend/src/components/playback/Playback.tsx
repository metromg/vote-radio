import React, { Component } from 'react';
import { connect } from 'react-redux';
import { Dispatch } from 'redux';

import { AppState } from '../../store';
import { CurrentSong } from '../../store/playback/types';
import { play, stop } from '../../store/playback/actions';
import { setErrorMessage } from '../../store/error/actions';
import Playbutton from './Playbutton';
import SongDescription from '../songInfo/SongDescription';
import VotingInfo from '../songInfo/VotingInfo';
import ProgressBar from './ProgressBar';
import CoverImage from '../songInfo/CoverImage';
import CoverBackgroundImage from '../songInfo/CoverBackgroundImage';
import AudioStream from './AudioStream';
import { apiBaseUrl, streamBaseUrl } from '../../config';
import './Playback.css';

interface PlaybackProps {
    currentSong: CurrentSong | null;
    playing: boolean;

    play: () => void;
    stop: () => void;
    setErrorMessage: (errorMessageKey: string) => void;
}

interface PlaybackState {
    loading: boolean;
    remainingDurationInSeconds: number;
}

class Playback extends Component<PlaybackProps, PlaybackState> {
    private interval?: number;

    constructor(props: PlaybackProps) {
        super(props);
        this.state = { loading: false, remainingDurationInSeconds: 0 };
    }

    componentDidMount() {
        this.updateRemainingDurationInSeconds();
        this.interval = window.setInterval(() => this.updateRemainingDurationInSeconds(), 1000);
    }

    componentWillUnmount() {
        window.clearInterval(this.interval);
    }

    render() {
        if (this.props.currentSong == null) {
            return null;
        }

        const streamUrl = streamBaseUrl + "/radio.mp3";
        const coverImageUrl = this.props.currentSong.coverImageId != null
            ? apiBaseUrl + "/api/image/getImageAsync?imageId=" + this.props.currentSong.coverImageId
            : null;

        return (
            <div className="playback-container">
                <CoverBackgroundImage url={coverImageUrl}>
                    <div className="playback">
                        <Playbutton 
                            playing={this.props.playing} 
                            loading={this.state.loading}
                            onClick={() => this.togglePlayback()} 
                        />
                        
                        <SongDescription 
                            title={this.props.currentSong.title} 
                            album={this.props.currentSong.album} 
                            artist={this.props.currentSong.artist} 
                        />
                        <VotingInfo voteCount={this.props.currentSong.voteCount} />

                        <ProgressBar 
                            totalDurationInSeconds={this.props.currentSong.durationInSeconds}
                            remainingDurationInSeconds={this.state.remainingDurationInSeconds}
                        />

                        <CoverImage url={coverImageUrl} />
                    </div>
                </CoverBackgroundImage>
                <AudioStream 
                    src={streamUrl} 
                    title={this.props.currentSong.title} 
                    playing={this.props.playing}
                    onLoadingAbort={() => this.setAudioStreamError()}
                    onLoadingStalled={() => this.setAudioStreamLoading()}
                    onLoadingSuspend={() => this.setAudioStreamLoading()}
                    onLoadingError={() => this.setAudioStreamError()}
                    onStreamWaiting={() => this.setAudioStreamLoading()}
                    onStreamPlaying={() => this.setAudioStreamLoading(false)}
                    onStreamTimeUpdate={() => this.setAudioStreamLoading(false)}
                    onStreamPaused={() => this.props.stop()}
                    onStreamEnded={() => this.setAudioStreamError()}
                />
            </div>
        );
    }

    private updateRemainingDurationInSeconds() {
        if (this.props.currentSong != null) {
            const endsAtTime = new Date(this.props.currentSong.endsAtTime).getTime();
            const currentTime = new Date().getTime();

            const remainingDurationInSeconds = Math.max((endsAtTime - currentTime) / 1000, 0);
            this.setState({ remainingDurationInSeconds });
        }
    }

    private togglePlayback() {
        if (this.props.playing) {
            this.props.stop();
        }
        else {
            this.props.play();
        }
    }

    private setAudioStreamLoading(loading = true) {
        if (this.state.loading != loading) {
            this.setState({ loading });
        }
    }

    private setAudioStreamError() {
        this.setAudioStreamLoading(false);
        this.props.stop();

        this.props.setErrorMessage("errorConnection");
    }
}

const mapStateToProps = (state: AppState) => ({
    currentSong: state.playback.currentSong,
    playing: state.playback.playing
});

const mapDispatchToProps = (dispatch: Dispatch) => ({
    play: () => dispatch(play()),
    stop: () => dispatch(stop()),
    setErrorMessage: (errorMessageKey: string) => dispatch(setErrorMessage(errorMessageKey))
});

export default connect(mapStateToProps, mapDispatchToProps)(Playback);