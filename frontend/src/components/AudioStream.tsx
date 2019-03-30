import React, { Component, RefObject } from 'react';

interface AudioProps {
    src: string;
    title?: string;

    playing: boolean;

    onLoadingAbort?: () => void;
    onLoadingStalled?: () => void;
    onLoadingSuspend?: () => void;
    onLoadingError?: () => void;
    onStreamWaiting?: () => void;
    onStreamPlaying?: () => void;
    onStreamEnded?: () => void;
}

class AudioStream extends Component<AudioProps> {
    private audioRef: RefObject<HTMLAudioElement>;

    constructor(props: AudioProps) {
        super(props);
        this.audioRef = React.createRef<HTMLAudioElement>();
    }

    getSnapshotBeforeUpdate(prevProps: AudioProps) {
        if (this.audioRef.current != null && this.props.playing == false && this.props.playing != prevProps.playing) {
            // stop the audio BEFORE the audio element is removed
            this.stopAudio(this.audioRef.current);
        }

        return null;
    }

    componentDidUpdate(prevProps: AudioProps) {
        if (this.audioRef.current != null && this.props.playing == true && this.props.playing != prevProps.playing) {
            // play the audio AFTER the audio element was added
            this.playAudio(this.audioRef.current);
        }
    }

    render() {
        const title = this.props.title != null
            ? this.props.title
            : this.props.src;

        if (this.props.playing) {
            return (
                <audio
                    ref={this.audioRef}
                    src={this.props.src}
                    title={title}
                    onAbort={() => this.props.onLoadingAbort && this.props.onLoadingAbort()}
                    onStalled={() => this.props.onLoadingStalled && this.props.onLoadingStalled()}
                    onSuspend={() => this.props.onLoadingSuspend && this.props.onLoadingSuspend()}
                    onError={() => this.props.onLoadingError && this.props.onLoadingError()}
                    onWaiting={() => this.props.onStreamWaiting && this.props.onStreamWaiting()}
                    onPlaying={() => this.props.onStreamPlaying && this.props.onStreamPlaying()}
                    onEnded={() => this.props.onStreamEnded && this.props.onStreamEnded()}
                >
                    Audio is not supported in your browser!
                </audio>
            );
        }
        else {
            return null;
        }
    }

    private playAudio(audioEl: HTMLAudioElement) {
        audioEl.play();
    }

    private stopAudio(audioEl: HTMLAudioElement) {
        audioEl.pause();
        audioEl.removeAttribute("src");
        try {
            // removing the src and loading stops the browser from streaming entirely.
            // if we wouldn't do that, the browser would continue to stream in the background
            audioEl.load();
        }
        catch (e) {
        }
    }
}

export default AudioStream;