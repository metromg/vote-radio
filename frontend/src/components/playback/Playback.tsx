import React, { Component } from 'react';
import { connect } from 'react-redux';
import { Dispatch } from 'redux';

import { AppState } from '../../store';
import { CurrentSong } from '../../store/playback/types';
import { loadCurrentSong, play, stop } from '../../store/playback/actions';
import Playbutton from './Playbutton';
import SongInfo from './SongInfo';
import ProgressBar from './ProgressBar';
import CoverImage from './CoverImage';
import AudioStream from './AudioStream';
import { apiBaseUrl, streamBaseUrl } from '../../config';
import './Playback.css';

interface PlaybackProps {
    currentSong: CurrentSong | null;
    playing: boolean;

    loadCurrentSong: () => void;
    play: () => void;
    stop: () => void;
}

class Playback extends Component<PlaybackProps> {
    componentDidMount() {
        this.props.loadCurrentSong();
    }

    render() {
        // TODO: Propper loading indicator
        if (this.props.currentSong == null) {
            return (<div>Loading...</div>);
        }

        const streamUrl = streamBaseUrl + "/radio.mp3";
        const coverImageUrl = this.props.currentSong.coverImageId != null
            ? apiBaseUrl + "/api/image/getImageAsync?imageId=" + this.props.currentSong.coverImageId
            : null;

        return (
            <div className="playback">
                <CoverImage url={coverImageUrl}>
                    <Playbutton 
                        playing={this.props.playing} 
                        onClick={() => this.togglePlayback()} 
                    />
                    <SongInfo 
                        title={this.props.currentSong.title} 
                        album={this.props.currentSong.album} 
                        artist={this.props.currentSong.artist} 
                        voteCount={this.props.currentSong.voteCount} 
                    />
                    <ProgressBar />
                </CoverImage>
                <AudioStream 
                    src={streamUrl} 
                    title={this.props.currentSong.title} 
                    playing={this.props.playing}
                    onLoadingAbort={() => this.props.stop()}
                    onLoadingError={() => this.props.stop()}
                    onStreamEnded={() => this.props.stop()}
                />
            </div>
        );
    }

    private togglePlayback() {
        if (this.props.playing) {
            this.props.stop();
        }
        else {
            this.props.play();
        }
    }
}

const mapStateToProps = (state: AppState) => ({
    currentSong: state.playback.currentSong,
    playing: state.playback.playing
});

const mapDispatchToProps = (dispatch: Dispatch) => ({
    loadCurrentSong: () => dispatch<any>(loadCurrentSong()),
    play: () => dispatch(play()),
    stop: () => dispatch(stop())
});

export default connect(mapStateToProps, mapDispatchToProps)(Playback);