import React, { Component } from 'react';

interface AudioProps {
    src: string;
    title: string;
}

interface AudioState {
    playing: boolean;
}

class Audio extends Component<AudioProps, AudioState> {
    private audioEl: HTMLAudioElement | null = null;

    constructor(props: AudioProps) {
        super(props);
        this.state = { playing: false };

        this.play = this.play.bind(this);
        this.stop = this.stop.bind(this);
        this.addEventListeners = this.addEventListeners.bind(this);
        this.removeEventListeners = this.removeEventListeners.bind(this);
        this.onAbort = this.onAbort.bind(this);
        this.onEnded = this.onEnded.bind(this);
        this.onPlaying = this.onPlaying.bind(this);
        this.onStalled = this.onStalled.bind(this);
        this.onSuspend = this.onSuspend.bind(this);
        this.onWaiting = this.onWaiting.bind(this);
        this.onTimeUpdate = this.onTimeUpdate.bind(this);
    }

    componentDidUpdate(prevProps: AudioProps, prevState: AudioState) {
        if (this.state.playing == true && this.audioEl != null && this.state.playing != prevState.playing) {
            this.addEventListeners(this.audioEl);
            this.audioEl.play();
        }
    }

    render() {
        const title = this.props.title != null
            ? this.props.title
            : this.props.src;
        
        let audio;
        if (this.state.playing) {
            audio = (
                <audio
                    ref={(ref) => this.audioEl = ref}
                    src={this.props.src}
                    title={title}
                >
                    Not Supported!
                </audio>
            );
        }
        else {
            audio = null;
        }

        return (
            <React.Fragment>
                {audio}
                <button onClick={() => this.play()}>Play</button>
                <button onClick={() => this.stop()}>Stop</button>
            </React.Fragment>
        );
    }

    private play() {
        this.setState({ playing: true });
    }

    private stop() {
        if (this.audioEl != null) {
            this.audioEl.pause();
            this.audioEl.removeAttribute("src");
            try {
                // removing the src and loading stops the browser from streaming entirely.
                // if we wouldn't do that, the browser would continue to stream in the background
                this.audioEl.load();
            }
            catch (e) {
            }

            this.removeEventListeners(this.audioEl);
        }

        this.setState({ playing: false });
    }

    private addEventListeners(audioEl: HTMLAudioElement) {
        audioEl.addEventListener("abort", this.onAbort);
        audioEl.addEventListener("ended", this.onEnded);
        audioEl.addEventListener("playing", this.onPlaying);
        audioEl.addEventListener("stalled", this.onStalled);
        audioEl.addEventListener("suspend", this.onSuspend);
        audioEl.addEventListener("waiting", this.onWaiting);
        audioEl.addEventListener("timeupdate", this.onTimeUpdate);
    }

    private removeEventListeners(audioEl: HTMLAudioElement) {
        audioEl.removeEventListener("abort", this.onAbort);
        audioEl.removeEventListener("ended", this.onEnded);
        audioEl.removeEventListener("playing", this.onPlaying);
        audioEl.removeEventListener("stalled", this.onStalled);
        audioEl.removeEventListener("suspend", this.onSuspend);
        audioEl.removeEventListener("waiting", this.onWaiting);
        audioEl.removeEventListener("timeupdate", this.onTimeUpdate);
    }

    private onAbort() {
        console.log("audio abort");

        // when the stream aborts due to aborted loading make sure to safely remove the audio element
        this.stop();
    }

    private onEnded() {
        console.log("audio ended");

        // when the stream ends due to a connection error make sure to safely remove the audio element
        this.stop();
    }

    private onPlaying() {
        console.log("audio playing");
    }
    
    private onStalled() {
        console.log("audio stalled");

        // here we could display some connection error
    }

    private onSuspend() {
        console.log("audio suspended");

        // here we could display some connection error
    }

    private onWaiting() {
        console.log("audio waiting");

        // here we could display some loading indicator
    }

    private onTimeUpdate() {
        // here we could hide the connection error
    }
}

export default Audio;