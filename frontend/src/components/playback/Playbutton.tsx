import React, { MouseEvent } from 'react';

import LoadingSpinner from '../LoadingSpinner';

interface PlaybuttonProps {
    playing: boolean;
    loading: boolean;

    onClick: (e: MouseEvent) => void;
}

const Playbutton = (props: PlaybuttonProps) => {
    return (
        <button type="button" className="playback-playbutton"
                onClick={e => props.onClick(e)}
                disabled={props.loading}>
            <PlaybuttonSymbol {...props} />
        </button>
    );
}

const PlaybuttonSymbol = (props: PlaybuttonProps) => {
    if (props.loading) {
        return <LoadingSpinner />;
    }

    return (
        props.playing
            ? <img src="/icons/icons8-stop-filled-50.png" />
            : <img src="/icons/icons8-play-filled-50.png" />
    );
}

export default Playbutton;